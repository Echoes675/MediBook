namespace MediBook.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using MediBook.Core.Enums;
    using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

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
        /// The Patient Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The Patient navigation property
        /// </summary>
        [Required]
        public Patient Patient { get; set; }

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
        /// The Id of the AppointmentSession
        /// </summary>
        public int AppointmentSlotId { get; set; }

        /// <summary>
        /// The AppointmentSlot navigation property
        /// </summary>
        [Required]
        public AppointmentSession AppointmentSlot { get; set; }
    }
}