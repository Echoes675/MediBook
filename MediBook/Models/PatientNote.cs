namespace MediBook.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
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
        /// The content of the PatientNote
        /// </summary>
        [Required, MaxLength(2500), MinLength(2)]
        public string Content { get; set; }

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
        [ForeignKey("UserId")]
        public int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The Employee navigation property
        /// </summary>
        public Employee MedicalPractitioner { get; set; }
    }
}