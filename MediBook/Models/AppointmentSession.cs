namespace MediBook.Core.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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
        [Required]
        public User User { get; set; }

        /// <summary>
        /// The duration of the AppointmentSession in minutes
        /// </summary>
        [Required]
        public int DurationInMins { get; set; }

        /// <summary>
        /// The timestamp of when the AppointmentSession begins
        /// </summary>
        [Required] 
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// The associated appointment slots
        /// </summary>
        public ICollection<AppointmentSlot> AppointmentSlots { get; set; }
    }
}