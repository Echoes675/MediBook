namespace MediBook.Core.DTOs
{
    using System;

    /// <summary>
    /// The Patient note
    /// </summary>
    [Serializable]
    public class PatientNoteDto
    {
        /// <summary>
        /// The PatientNote Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The encrypted content of the PatientNote
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The Patient Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The Id of the associated Medical Practitioner User account
        /// </summary>
        public int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The Medical Practitioner's Title
        /// </summary>
        public string MedicalPractitionerTitle { get; set; }

        /// <summary>
        /// The Medical Practitioner's Firstname
        /// </summary>
        public string MedicalPractitionerFirstname { get; set; }

        /// <summary>
        /// The Medical Practitioner's Lastname
        /// </summary>
        public string MedicalPractitionerLastname { get; set; }
    }
}