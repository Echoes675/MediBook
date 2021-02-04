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
    /// The AppointmentSessions Manager
    /// </summary>
    public class AppointmentSessionManager : IAppointmentSessionManager
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AppointmentSessionManager> _log;

        /// <summary>
        /// The AppointmentSession Dal
        /// </summary>
        private readonly IAppointmentSessionDal _apptSessionDal;

        private readonly IAppointmentSlotDal _apptSlotDal;

        /// <summary>
        /// The User Dal
        /// </summary>
        private readonly IUserDal _userDal;

        /// <summary>
        /// Initializes an instance of the <see cref="AppointmentSessionManager"/> class
        /// </summary>
        /// <param name="log"></param>
        /// <param name="apptSessionDal"></param>
        /// <param name="apptSlotDal"></param>
        /// <param name="userDal"></param>
        public AppointmentSessionManager(
            ILogger<AppointmentSessionManager> log,
            IAppointmentSessionDal apptSessionDal,
            IAppointmentSlotDal apptSlotDal,
            IUserDal userDal)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _apptSessionDal = apptSessionDal ?? throw new ArgumentNullException(nameof(apptSessionDal));
            _apptSlotDal = apptSlotDal ?? throw new ArgumentNullException(nameof(apptSlotDal));
            _userDal = userDal ?? throw new ArgumentNullException(nameof(userDal));
        }

        /// <summary>
        /// Creates and saves a new Appointment Session and Appointment Slots per the config specification
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<AppointmentBookResults> CreateNewAppointmentSessionAsync(AppointmentSessionConfiguration config)
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
        public async Task<AppointmentBookResults> GetAppointmentBookSessionsAsync(int userId, DateTime date)
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
        /// Deletes the selected AppointmentSession
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public async Task<AppointmentBookResults> DeleteAppointmentSessionAsync(int sessionId)
        {
            if (sessionId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sessionId));
            }

            var session = await _apptSessionDal.GetEntityAsync(sessionId);

            if (session != null)
            {
                return await DeleteAppointmentSessionAsync(session);
            }

            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Failed
            };
        }

        /// <summary>
        /// Deletes the selected AppointmentSession
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task<AppointmentBookResults> DeleteAppointmentSessionAsync(AppointmentSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (session.AppointmentSlots == null)
            {
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            // Delete each of the slots that are associated with the session
            var slotDeleteSuccess = true;
            foreach (var slot in session.AppointmentSlots)
            {
                if (! await _apptSlotDal.DeleteAsync(slot))
                {
                    slotDeleteSuccess = false;
                }
            }

            // If the deletion of the Session's slots failed, return
            if (!slotDeleteSuccess)
            {
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Failed
                };
            }

            // Delete the Session itself
            if (await _apptSessionDal.DeleteAsync(session.Id))
            {
                // If deletion is successful, return success result
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.Success
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
                    State = SlotState.Available,
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
    }
}