namespace MediBook.Core.DTOs
{
    using System;

    public interface IPatientRecordEntry
    {
        /// <summary>
        /// The timestamp
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// The Patient Id
        /// </summary>
        int PatientId { get; set; }

        /// <summary>
        /// The Id of the associated Medical Practitioner User account
        /// </summary>
        int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The Medical Practitioner's Title
        /// </summary>
        string MedicalPractitionerTitle { get; set; }

        /// <summary>
        /// The Medical Practitioner's Firstname
        /// </summary>
        string MedicalPractitionerFirstname { get; set; }

        /// <summary>
        /// The Medical Practitioner's Lastname
        /// </summary>
        string MedicalPractitionerLastname { get; set; }
    }
}