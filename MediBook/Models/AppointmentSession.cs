namespace MediBook.Core.Models
{
    using System;

    /// <summary>
    /// The Appointment Session
    /// </summary>
    public class AppointmentSession : IDbEntity
    {
        /// <summary>
        /// The AppointmentSession Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Id of the associated Medical Practitioner User account
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The User navigation property
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The duration of the AppointmentSession in minutes
        /// </summary>
        public int DurationInMins { get; set; }

        /// <summary>
        /// The timestamp of when the AppointmentSession begins
        /// </summary>
        public DateTime StartDateTime { get; set; }
    }
}