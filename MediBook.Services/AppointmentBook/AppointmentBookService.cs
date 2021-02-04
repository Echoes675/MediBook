namespace MediBook.Services.AppointmentBook
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Data.Repositories;
    using MediBook.Services.Enums;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The Appointment Book Service
    /// </summary>
    public class AppointmentBookService : IAppointmentBookService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AppointmentBookService> _log;

        /// <summary>
        /// The AppointmentSession Manager
        /// </summary>
        private readonly IAppointmentSessionManager _sessionMgr;

        /// <summary>
        /// The AppointmentBookingManager
        /// </summary>
        private readonly IAppointmentBookingManager _apptBooking;

        /// <summary>
        /// The AppointmentSession Dal
        /// </summary>
        private readonly IAppointmentSessionDal _apptSessionDal;

        /// <summary>
        /// Initializes an instance of the <see cref="AppointmentBookService"/>
        /// </summary>
        public AppointmentBookService(
            ILogger<AppointmentBookService> log,
            IAppointmentSessionManager sessionMgr,
            IAppointmentBookingManager apptBooking,
            IAppointmentSessionDal apptSessionDal)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _sessionMgr = sessionMgr ?? throw new ArgumentNullException(nameof(sessionMgr));
            _apptBooking = apptBooking ?? throw new ArgumentNullException(nameof(apptBooking));
            _apptSessionDal = apptSessionDal ?? throw new ArgumentNullException(nameof(apptSessionDal));
        }

        /// <summary>
        /// Creates and saves a new Appointment Session and Appointment Slots per the config specification
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public Task<AppointmentBookResults> CreateNewAppointmentSession(AppointmentSessionConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            return _sessionMgr.CreateNewAppointmentSessionAsync(config);
        }

        /// <summary>
        /// Gets all the AppointmentBookSessions on a given day. If the calling user is a MedicalPractitioner they
        /// will only see their own sessions only. If they are Reception or Practice Admin they will see the
        /// appointment books for all staff
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public Task<AppointmentBookResults> GetAppointmentBookSessions(int userId, DateTime date)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            return _sessionMgr.GetAppointmentBookSessionsAsync(userId, date);
        }

        public async Task<AppointmentBookResults> CancelAppointmentBookSession(int sessionId)
        {
            if (sessionId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sessionId));
            }

            var session = await _apptSessionDal.GetEntityAsync(sessionId);
            if (session == null)
            {
                return new AppointmentBookResults()
                {
                    ResultCode = ServiceResultStatusCode.NotFound
                };
            }

            AppointmentBookResults result;
            var appointmentsCancelSuccess = true;
            foreach (var slot in session?.AppointmentSlots)
            {
                result = await _apptBooking.CancelAppointmentAsync(slot);
                if (result.ResultCode != ServiceResultStatusCode.Success)
                {
                    appointmentsCancelSuccess = false;
                }
            }

            // Once cancellation of the booked appointments is successful, then delete the Session and slots
            if (appointmentsCancelSuccess)
            {
                return await _sessionMgr.DeleteAppointmentSessionAsync(session);
            }

            return new AppointmentBookResults()
            {
                ResultCode = ServiceResultStatusCode.Failed
            };
        }

        /// <summary>
        /// Gets the Appointment history for a given patient where the calling user is a MedicalPractitioner and was
        /// also associated with the appointment
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<AppointmentBookResults> GetPatientAppointmentHistory(int patientId, int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (patientId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(patientId));
            }

            return _apptBooking.GetPatientAppointmentHistory(patientId, userId);
        }



        /// <summary>
        /// Book a new appointment for a patient
        /// </summary>
        /// <returns></returns>
        public Task<AppointmentBookResults> BookAppointmentAsync(AddOrUpdateAppointmentData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return _apptBooking.BookAppointmentAsync(data);
        }

        /// <summary>
        /// Update an existing appointment for a patient
        /// </summary>
        /// <returns></returns>
        public Task<AppointmentBookResults> UpdateAppointmentAsync(AddOrUpdateAppointmentData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return _apptBooking.UpdateAppointmentAsync(data);
        }

        /// <summary>
        /// Cancel a booked appointment for a patient
        /// </summary>
        /// <returns></returns>
        public Task<AppointmentBookResults> CancelAppointmentAsync(int slotId)
        {
            if (slotId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(slotId));
            }

            return _apptBooking.CancelAppointmentAsync(slotId);
        }
    }
}