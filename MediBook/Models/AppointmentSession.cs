namespace MediBook.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
        /// The Id of the associated Medical Practitioner Employee account
        /// </summary>
        [ForeignKey("UserId")]
        public int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The Employee navigation property
        /// </summary>
        [Required]
        public Employee MedicalPractitioner { get; set; }

        /// <summary>
        /// The associated appointments
        /// </summary>
        public ICollection<Appointment> Appointments { get; set; }
    }
}