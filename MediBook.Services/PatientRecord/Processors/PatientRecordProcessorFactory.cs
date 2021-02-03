namespace MediBook.Services.PatientRecord.Processors
{
    using System;
    using MediBook.Core.DTOs;
    using Microsoft.Extensions.Logging;

    public class PatientRecordProcessorFactory : IPatientRecordProcessorFactory
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<PatientNoteProcessor> _log;

        /// <summary>
        /// The Patient Note Processor
        /// </summary>
        private readonly IPatientNoteProcessor _patientNoteProcessor;

        public PatientRecordProcessorFactory(ILogger<PatientNoteProcessor> log, IPatientNoteProcessor patientNoteProcessor)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _patientNoteProcessor = patientNoteProcessor ?? throw new ArgumentNullException(nameof(patientNoteProcessor));
        }

        /// <summary>
        /// Gets the appropriate Processor for the type of PatientRecord
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IPatientRecordProcessor<TEntity> GetPatientRecordProcessor<TEntity>() where TEntity : class, IPatientRecordEntry
        {
            //Get the type of PatientRecordEntry
            var type = typeof(TEntity);

            if (type == typeof(PatientNoteDto))
            {
                return _patientNoteProcessor as IPatientRecordProcessor<TEntity>;
            }

            return null;
        }
    }
}