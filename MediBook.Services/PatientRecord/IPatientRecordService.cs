namespace MediBook.Services.PatientRecord
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;

    public interface IPatientRecordService
    {
        /// <summary>
        /// Adds a new PatientRecordEntry
        /// </summary>
        /// <param name="newPatientRecordEntry"></param>
        /// <returns></returns>
        Task<TEntity> AddPatientRecordEntryAsync<TEntity>(TEntity newPatientRecordEntry) where TEntity : class, IPatientRecordEntry;

        /// <summary>
        /// Retrieves the patient records for a medical practitioner was the author
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        List<TEntity> RetrievePatientRecords<TEntity>(int patientId, int medicalPractitionerId) where TEntity : class, IPatientRecordEntry;

        /// <summary>
        /// Retrieves an individual patient record for a medical practitioner where they were the author
        /// </summary>
        /// <param name="patientRecordId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        TEntity RetrievePatientRecord<TEntity>(int patientRecordId, int medicalPractitionerId) where TEntity : class, IPatientRecordEntry;
    }
}