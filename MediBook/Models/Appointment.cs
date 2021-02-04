namespace MediBook.Core.Models
{
    using System.ComponentModel.DataAnnotations;
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
        /// The Appointment state
        /// </summary>
        public AppointmentState State { get; set; }

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
        /// The Id of the associated Medical Practitioner MedicalPractitioner account
        /// </summary>
        public int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The MedicalPractitioner Employee navigation property
        /// </summary>
        [Required]
        public Employee MedicalPractitioner { get; set; }
    }
}