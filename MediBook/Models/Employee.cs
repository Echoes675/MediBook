namespace MediBook.Core.Models
{
    using MediBook.Core.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The Employee account
    /// </summary>
    public class Employee : IDbEntity
    {
        /// <summary>
        /// The Employee Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The user's title
        /// </summary>
        [Required]
        public Title Title { get; set; }

        /// <summary>
        /// The user's firstname
        /// </summary>
        [Required, MaxLength(50), MinLength(2)]
        public string Firstname { get; set; }

        /// <summary>
        /// The user's lastname
        /// </summary>
        [Required, MaxLength(50), MinLength(2)]
        public string Lastname { get; set; }

        /// <summary>
        /// Navigation property for patient notes this user has made
        /// </summary>
        public ICollection<PatientNote> PatientNotes { get; set; } = new List<PatientNote>();

        /// <summary>
        /// Navigation property for the associated AppointmentSessions
        /// </summary>
        public ICollection<AppointmentSession> AppointmentSessions { get; set; }

        /// <summary>
        /// Navigation property for the associated Appointments
        /// </summary>
        public ICollection<Appointment> Appointments { get; set; }
    }
}