namespace MediBook.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The many to many relationship between Patients and MedicalPractitioners
    /// </summary>
    [Index(nameof(PatientId))]
    [Index(nameof(MedicalPractitionerId))]
    public class PatientsMedicalPractitioner : IDbEntity
    {
        /// <summary>
        /// The PatientsMedicalPractitioner Id
        /// </summary>
        public int Id { get; set; }

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
        public Employee MedicalPractitioner { get; set; }
    }
}