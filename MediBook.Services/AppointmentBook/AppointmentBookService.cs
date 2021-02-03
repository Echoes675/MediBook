namespace MediBook.Services.AppointmentBook
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;
    using MediBook.Data.Repositories;
    using MediBook.Services.Enums;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The Appointment Book Service
    /// </summary>
    public class AppointmentBookService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AppointmentBookService> _log;

        /// <summary>
        /// The Appointment Session Dal
        /// </summary>
        private readonly IAppointmentSessionDal _apptSessionDal;

        /// <summary>
        /// The user dal
        /// </summary>
        private readonly IUserDal _userDal;

        /// <summary>
        /// The Appointment Dal
        /// </summary>
        private readonly IAppointmentDal _apptDal;

        /// <summary>
        /// The Appointment Slot Dal
        /// </summary>
        private readonly IAppointmentSlotDal _apptSlotDal;

        /// <summary>
        /// Initializes an instance of the <see cref="AppointmentBookService"/>
        /// </summary>
        public AppointmentBookService(ILogger<AppointmentBookService> log, IAppointmentSessionDal apptSessionDal, IUserDal userDal, IAppointmentDal apptDal, IAppointmentSlotDal apptSlotDal)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _apptSessionDal = apptSessionDal ?? throw new ArgumentNullException(nameof(apptSessionDal));
            _userDal = userDal ?? throw new ArgumentNullException(nameof(userDal));
            _apptDal = apptDal ?? throw new ArgumentNullException(nameof(apptDal));
            _apptSlotDal = apptSlotDal ?? throw new ArgumentNullException(nameof(apptSlotDal));
        }

        /// <summary>
        /// Creates and saves a new Appointment Session and Appointment Slots per the config specification
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<AppointmentBookResults> CreateNewAppointmentSession(AppointmentSessionConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            // Generate the new session and slots per the config
            var newSession = GenerateNewAppointmentSession(config);

            //Attempt to save the new session to the db
            var result = await _apptSessionDal.AddAsync(newSession);

            if (result == null)
            {
                // Could not create the new session as another already exists in the same time block for the same Medical Practitioner
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.AlreadyExists
                };
            }

            // Convert the saved session to an AppointmentSessionDetails object for return
            var sessionDetails = new AppointmentSessionDetails(newSession);

            // Return results
            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Success,
                AppointmentSessionDetails = new List<AppointmentSessionDetails>()
                {
                    sessionDetails
                }
            };
        }

        /// <summary>
        /// Gets all the AppointmentBookSessions on a given day. If the calling user is a MedicalPractitioner they
        /// will only see their own sessions only. If they are Reception or Practice Admin they will see the
        /// appointment books for all staff
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<AppointmentBookResults> GetAppointmentBookSessions(int userId, DateTime date)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            // Get the account of the calling user so we can determine their role
            var callingUser = await _userDal.GetEntityAsync(userId);

            if (callingUser.JobDescription == null)
            {
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            List<AppointmentSession> sessions;
            if (callingUser.JobDescription.Role == UserRole.MedicalPractitioner)
            {
                // If this has been called by a Medical Practitioner, only return their own sessions for the specified day
                sessions = await _apptSessionDal.FilterAndOrderAsync((x =>
                    x.StartDateTime.Date == date.Date && x.MedicalPractitionerId == userId), s => s.StartDateTime);
            }
            else
            {
                // If this has been called by Reception or Practice Admin staff, return all sessions  the specified day
                sessions = await _apptSessionDal.FilterAndOrderAsync((x =>
                    x.StartDateTime.Date == date.Date), s => s.StartDateTime);
            }

            if (!sessions.Any())
            {
                // no sessions found for the given date (and Medical practitioner)
                // return success and no sessions
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Success
                };
            }

            // Get the details of the Medical Practitioners associated with these sessions
                var medicalPractitionersIds = sessions.Select(i => i.MedicalPractitionerId).ToList();
            var medicalPractitioners = await _userDal.GetEntitiesAsync(medicalPractitionersIds);

            // Join the list of Appointment Sessions with the list of Medical Practitioner's User accounts and create a list of
            // AppointmentSessionDetails ready for return
            var sessionDetails = sessions.Join(
                medicalPractitioners,
                session => session.MedicalPractitionerId,
                medicalPractitioner => medicalPractitioner.Id,
                (session, medicalPractitioner) => new AppointmentSessionDetails(session, medicalPractitioner)).ToList();

            // return success and the sessionDetails
            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Success,
                AppointmentSessionDetails = sessionDetails
            };
        }

        /// <summary>
        /// Gets the Appointment history for a given patient where the calling user is a MedicalPractitioner and was
        /// also associated with the appointment
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AppointmentBookResults> GetPatientAppointmentHistory(int patientId, int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            // Get the account of the calling user so we can determine their role
            var callingUser = await _userDal.GetEntityAsync(userId);

            if (callingUser.JobDescription == null)
            {
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            List<AppointmentSlot> appointmentSlots;
            if (callingUser.JobDescription.Role == UserRole.MedicalPractitioner)
            {
                // If this has been called by a Medical Practitioner, only return their own sessions for the specified day
                appointmentSlots = _apptSlotDal.Filter(x => x.Appointment.PatientId == patientId && x.Appointment.MedicalPractitionerId == userId).ToList();


                var appointmentDetails = appointmentSlots.Select(x => new AppointmentDetails(callingUser, x)).OrderByDescending(x => x.AppointmentDateTime).ToList();
                var result = new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Success,
                    AppointmentsDetails = appointmentDetails
                };
            }

            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Failed
            };
        }

        /// <summary>
        /// Generates the Appointment Session and Appointment Slots per the config specification
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private AppointmentSession GenerateNewAppointmentSession(AppointmentSessionConfiguration config)
        {
            var slotDuration = config.DurationInMins / config.NumberOfAppointmentSlots;
            var slots = new List<AppointmentSlot>();
            var appointmentDateTime = config.StartDateTime;
            for (var i = 0; i < config.NumberOfAppointmentSlots; i++)
            {
                slots.Add(new AppointmentSlot()
                {
                    State = AppointmentState.AvailableToBook,
                    AppointmentDateTime = appointmentDateTime,
                    AppointmentDurationInMins = slotDuration,
                });

                //Advance the start time of the next appointment
                appointmentDateTime = appointmentDateTime.AddMinutes(slotDuration);
            }

            var session = new AppointmentSession()
            {
                DurationInMins = config.DurationInMins,
                StartDateTime = config.StartDateTime,
                MedicalPractitionerId = config.MedicalPractitionerId,
                AppointmentSlots = slots
            };

            return session;
        }

        /// <summary>
        /// Book a new appointment for a patient
        /// </summary>
        /// <returns></returns>
        public async Task<AppointmentBookResults> BookAppointmentAsync(BookAppointmentData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            throw new NotImplementedException();
            // Get Appointment Slot

            // Get Medical Practitioner

            // Get 
        }
    }

    /// <summary>
    /// Data required to book a new appointment
    /// </summary>
    public class BookAppointmentData
    {
        /// <summary>
        /// The patient Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The Id of the selected slot
        /// </summary>
        public int SlotId { get; set; }

        /// <summary>
        /// The Medical Practitioner Id
        /// </summary>
        public int MedicalPractitionerId { get; set; }
    }
}