namespace MediBook.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

    /// <summary>
    /// The Patient Note
    /// </summary>
    [Index(nameof(PatientId))]
    [Index(nameof(MedicalPractitionerId))]
    public class PatientNote : IDbEntity
    {
        /// <summary>
        /// The PatientNote Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The encrypted content of the PatientNote
        /// </summary>
        [Required]
        public byte[] Content { get; set; }

        /// <summary>
        /// The timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The Patient Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The Patient navigation property
        /// </summary>
        public Patient Patient { get; set; }

        /// <summary>
        /// The Id of the associated Medical Practitioner Employee account
        /// </summary>
        public int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The Employee navigation property
        /// </summary>
        public User MedicalPractitioner { get; set; }
    }
}