namespace MediBook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediBook.Core.Models;

    public interface IAppointmentSessionDal
    {
        /// <summary>
        /// Returns all Appointment Sessions including associated Appointment Slots and Appointments
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AppointmentSession>> GetAllAsync();

        /// <summary>
        /// Filters and orders the list of Appointment Sessions per the requirements. Includes Appointment Slots and Appointments
        /// </summary>
        /// <param name="appointmentSessionWhere"></param>
        /// <param name="appointmentSessionOrderBy"></param>
        /// <returns></returns>
        Task<List<AppointmentSession>> FilterAndOrderAsync(Func<AppointmentSession, bool> appointmentSessionWhere, Func<AppointmentSession, DateTime> appointmentSessionOrderBy);

        /// <summary>
        /// Method to Add an Entity to the database after confirming it doesn't already exist
        /// </summary>
        /// <param name="entity">The entity parameter</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<AppointmentSession> AddAsync(AppointmentSession entity);

        /// <summary>
        /// Method to delete an entity from the database if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Method to Update an Entity to the database after confirming it doesn't already exist
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<AppointmentSession> UpdateAsync(AppointmentSession entity);

        /// <summary>
        /// Returns an entity of a given type using its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppointmentSession> GetEntityAsync(int id);

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<bool> CheckEntityExistsAsync(AppointmentSession entity);

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> CheckEntityExistsAsync(int id);

        /// <summary>
        /// Verifies whether an Appointment session already exists in the time block for a given Medical Practitioner
        /// </summary>
        /// <param name="medicalPractitionerId"></param>
        /// <param name="sessionStartTime"></param>
        /// <param name="durationInMins"></param>
        /// <returns></returns>
        public Task<bool> CheckEntityExistsAsync(int medicalPractitionerId, DateTime sessionStartTime, int durationInMins);

        /// <summary>
        /// Filter entities in a DbSet based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.InvalidOperationException">Condition.</exception>
        IEnumerable<AppointmentSession> Filter(Func<AppointmentSession, bool> predicate);

        /// <summary>
        /// Returns a list of a patient's appointment slots filtered to the calling Medical Practitioner's Id
        /// </summary>
        /// <returns></returns>
        Task<List<AppointmentSlot>> GetPatientAppointmentSlotsAssociatedWithMedicalPractitionerSessions(int userId, int patientId);
    }
}