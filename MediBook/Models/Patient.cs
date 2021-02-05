namespace MediBook.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MediBook.Core.Enums;

    /// <summary>
    /// The patient
    /// </summary>
    public class Patient : IDbEntity
    {
        /// <summary>
        /// The Patient's Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The patient's title
        /// </summary>
        [Required]
        public Title Title { get; set; }

        /// <summary>
        /// The patient's firstname
        /// </summary>
        [Required, MaxLength(50)]
        public string Firstname { get; set; }

        /// <summary>
        /// The patient's lastname
        /// </summary>
        [Required, MaxLength(50)]
        public string Lastname { get; set; }

        /// <summary>
        /// The patient's date of birth
        /// </summary>
        [Required] 
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// The patient's Health and Care number
        /// </summary>
        public long HealthAndCare { get; set; }

        /// <summary>
        /// The first line of the patient's address
        /// </summary>
        [Required, MaxLength(50)] 
        public string Address1 { get; set; }

        /// <summary>
        /// The second line of the patient's address
        /// </summary>
        [MaxLength(50)] 
        public string Address2 { get; set; }

        /// <summary>
        /// The city of the patient's address
        /// </summary>
        [MaxLength(50)]
        public string City { get; set; }

        /// <summary>
        /// The county of the patient's address
        /// </summary>
        [MaxLength(50)]
        public string County { get; set; }

        /// <summary>
        /// The patient's post code
        /// </summary>
        [Required, MaxLength(8)] 
        public string PostCode { get; set; }

        /// <summary>
        /// The patient's phone number
        /// </summary>
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The patient's mobile phone number
        /// </summary>
        [MaxLength(20)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// The patient's email address
        /// </summary>
        [MaxLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// The patient's status
        /// </summary>
        public PatientStatus Status { get; set; }

        /// <summary>
        /// The many to many relationship between Patients and MedicalPractitioners
        /// </summary>
        public ICollection<PatientsMedicalPractitioner> PatientsMedicalPractitioners { get; set; }

        /// <summary>
        /// The patient's notes
        /// </summary>
        public ICollection<PatientNote> PatientNotes { get; set; }

        /// <summary>
        /// The patient's appointments
        /// </summary>
        public ICollection<AppointmentSlot> AppointmentSlots { get; set; }

        public PatientUser PatientUserAccount { get; set; }
    }
}