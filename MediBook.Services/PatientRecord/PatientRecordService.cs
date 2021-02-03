namespace MediBook.Services.PatientRecord
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Services.PatientRecord.Processors;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The Patient record service
    /// </summary>
    public class PatientRecordService : IPatientRecordService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<PatientRecordService> _log;

        /// <summary>
        /// The Patient Record Processor Factory
        /// </summary>
        private readonly IPatientRecordProcessorFactory _recordProcessorFactory;

        /// <summary>
        /// Initialize an instance of the patient record service
        /// </summary>
        public PatientRecordService(ILogger<PatientRecordService> log, IPatientRecordProcessorFactory recordProcessorFactory)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _recordProcessorFactory = recordProcessorFactory ?? throw new ArgumentNullException(nameof(recordProcessorFactory));
        }

        /// <summary>
        /// Adds a new PatientRecordEntry
        /// </summary>
        /// <param name="newPatientRecordEntry"></param>
        /// <returns></returns>
        public async Task<TEntity> AddPatientRecordEntryAsync<TEntity>(TEntity newPatientRecordEntry) where TEntity : class, IPatientRecordEntry
        {
            if (newPatientRecordEntry == null)
            {
                throw new ArgumentNullException(nameof(newPatientRecordEntry));
            }

            // Get the appropriate PatientRecordProcessor for the type of PatientRecordEntry
            var processor = _recordProcessorFactory.GetPatientRecordProcessor<TEntity>();

            // Call the processor to add the PatientRecordEntry
            var result = await processor.AddPatientRecordEntryAsync(newPatientRecordEntry);

            return result;
        }

        /// <summary>
        /// Retrieves the patient records for a medical practitioner was the author
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        public List<TEntity> RetrievePatientRecords<TEntity>(int patientId, int medicalPractitionerId) where TEntity : class, IPatientRecordEntry
        {
            if (patientId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(patientId));
            }

            if (medicalPractitionerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(medicalPractitionerId));
            }

            // Get the appropriate PatientRecordProcessor for the type of PatientRecordEntry
            var processor = _recordProcessorFactory.GetPatientRecordProcessor<TEntity>();

            // Call the processor to collect the PatientRecordEntries
            var result = processor.RetrievePatientRecords(patientId, medicalPractitionerId);

            return result;
        }

        /// <summary>
        /// Retrieves an individual patient record for a medical practitioner where they were the author
        /// </summary>
        /// <param name="patientRecordId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        public TEntity RetrievePatientRecord<TEntity>(int patientRecordId, int medicalPractitionerId) where TEntity : class, IPatientRecordEntry
        {
            if (patientRecordId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(patientRecordId));
            }

            if (medicalPractitionerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(medicalPractitionerId));
            }

            // Get the appropriate PatientRecordProcessor for the type of PatientRecordEntry
            var processor = _recordProcessorFactory.GetPatientRecordProcessor<TEntity>();

            // Call the processor to collect the PatientRecordEntries
            var result = processor.RetrievePatientRecord(patientRecordId, medicalPractitionerId);

            return result;
        }
    }
}