namespace MediBook.Services.PatientRecord.Processors
{
    using MediBook.Core.DTOs;

    public interface IPatientRecordProcessorFactory
    {
        /// <summary>
        /// Gets the appropriate Processor for the type of PatientRecord
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IPatientRecordProcessor<TEntity> GetPatientRecordProcessor<TEntity>()
            where TEntity : class, IPatientRecordEntry;
    }
}