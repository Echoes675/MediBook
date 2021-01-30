namespace MediBook.Core.Models
{
    using System;
    using System.Collections.Generic;
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
        public Title Title { get; set; }

        /// <summary>
        /// The patient's firstname
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// The patient's lastname
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// The patient's date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// The patient's Health and Care number
        /// </summary>
        public long HealthAndCare { get; set; }

        /// <summary>
        /// The first line of the patient's address
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// The second line of the patient's address
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// The city of the patient's address
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The county of the patient's address
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// The patient's post code
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// The patient's phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The patient's mobile phone number
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// The patient's email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The MedicalPractitioners the patient is associated with
        /// </summary>
        public ICollection<PatientsMedicalPractitioners> MedicalPractitioners { get; set; }

        /// <summary>
        /// The patient's notes
        /// </summary>
        public ICollection<PatientNote> PatientNotes { get; set; }
    }
}