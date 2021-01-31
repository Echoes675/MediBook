namespace MediBook.Data.Repositories
{
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// PatientNoteDal
    /// </summary>
    public class PatientNoteDal : RepositoryBase<PatientNote>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientNoteDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public PatientNoteDal(IDatabaseContext databaseContext, ILogger<PatientNoteDal> logger) : base(databaseContext, logger)
        {
        }
    }
}