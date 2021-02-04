namespace MediBook.Services.AppointmentBook
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Core.Models;

    public interface IAppointmentSessionManager
    {
        /// <summary>
        /// Creates and saves a new Appointment Session and Appointment Slots per the config specification
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        Task<AppointmentBookResults> CreateNewAppointmentSessionAsync(AppointmentSessionConfiguration config);

        /// <summary>
        /// Gets all the AppointmentBookSessions on a given day. If the calling user is a MedicalPractitioner they
        /// will only see their own sessions only. If they are Reception or Practice Admin they will see the
        /// appointment books for all staff
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<AppointmentBookResults> GetAppointmentBookSessionsAsync(int userId, DateTime date);

        /// <summary>
        /// Deletes the selected AppointmentSession and any associated AppointmentSlots and booked Appointments
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task<AppointmentBookResults> DeleteAppointmentSessionAsync(int sessionId);

        /// <summary>
        /// Deletes the selected AppointmentSession and any associated AppointmentSlots and booked Appointments
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        Task<AppointmentBookResults> DeleteAppointmentSessionAsync(AppointmentSession session);
    }
}