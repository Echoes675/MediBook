namespace MediBook.Services.PatientRecord.Processors
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;

    public interface IPatientNoteProcessor : IPatientRecordProcessor<PatientNoteDto>
    {
        /// <summary>
        /// Adds a new PatientRecordEntry
        /// </summary>
        /// <param name="newPatientRecordEntry"></param>
        /// <returns></returns>
        new Task<PatientNoteDto> AddPatientRecordEntryAsync(PatientNoteDto newPatientRecordEntry);

        /// <summary>
        /// Retrieves the patient records for a medical practitioner was the author
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        new List<PatientNoteDto> RetrievePatientRecords(int patientId, int medicalPractitionerId);

        /// <summary>
        /// Retrieves an individual patient record for a medical practitioner where they were the author
        /// </summary>
        /// <param name="patientRecordId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        new PatientNoteDto RetrievePatientRecord(int patientRecordId, int medicalPractitionerId);
    }
}