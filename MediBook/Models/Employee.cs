namespace MediBook.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MediBook.Core.Enums;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The Employee account
    /// </summary>
    [Index(nameof(JobDescriptionId))]
    public class Employee : IDbEntity
    {
        /// <summary>
        /// The Employee Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Employee Guid
        /// </summary>
        public Guid EmployeeGuid { get; set; }

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
        /// The Id of the user's Job Description
        /// </summary>
        public int JobDescriptionId { get; set; }

        /// <summary>
        /// The navigation property for the user's Job Description
        /// </summary>
        [Required]
        public JobDescription JobDescription { get; set; }

        /// <summary>
        /// The many to many relationship between Patients and MedicalPractitioners
        /// </summary>
        public ICollection<PatientsMedicalPractitioner> PatientsMedicalPractitioners { get; set; }

        /// <summary>
        /// Navigation property for patient notes this user has made
        /// </summary>
        public ICollection<PatientNote> PatientNotes { get; set; }

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