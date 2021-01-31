namespace MediBook.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using MediBook.Core.Enums;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The booked Appointment
    /// </summary>
    [Index(nameof(PatientId))]
    [Index(nameof(MedicalPractitionerId))]
    public class Appointment : IDbEntity
    {
        /// <summary>
        /// The Appointment Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Appointment State
        /// </summary>
        [Required]
        public AppointmentState State { get; set; }

        /// <summary>
        /// The day and time of the appointment
        /// </summary>
        public DateTime AppointmentDateTime { get; set; }

        /// <summary>
        /// The Appointment duration in minutes
        /// </summary>
        public int AppointmentDurationInMins { get; set; }

        /// <summary>
        /// The Patient Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The Patient navigation property
        /// </summary>
        [Required]
        public Patient Patient { get; set; }

        /// <summary>
        /// The Id of the associated Medical Practitioner User account
        /// </summary>
        [ForeignKey("UserId")]
        public int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The User navigation property
        /// </summary>
        [Required]
        public User MedicalPractitioner { get; set; }

        /// <summary>
        /// The Id of the AppointmentSession
        /// </summary>
        public int AppointmentSessionId { get; set; }

        /// <summary>
        /// The AppointmentSession navigation property
        /// </summary>
        [Required]
        public AppointmentSession AppointmentSession { get; set; }
    }
}