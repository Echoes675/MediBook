namespace MediBook.Services.PatientRecord.Processors
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;

    public interface IPatientRecordProcessor<TEntity> where TEntity : class, IPatientRecordEntry
    {
        /// <summary>
        /// Adds a new PatientRecordEntry
        /// </summary>
        /// <param name="newPatientRecordEntry"></param>
        /// <returns></returns>
        public Task<TEntity> AddPatientRecordEntryAsync(TEntity newPatientRecordEntry);

        /// <summary>
        /// Retrieves the patient records for a medical practitioner was the author
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        public List<TEntity> RetrievePatientRecords(int patientId, int medicalPractitionerId);

        /// <summary>
        /// Retrieves an individual patient record for a medical practitioner where they were the author
        /// </summary>
        /// <param name="patientRecordId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        public TEntity RetrievePatientRecord(int patientRecordId, int medicalPractitionerId);
    }
}