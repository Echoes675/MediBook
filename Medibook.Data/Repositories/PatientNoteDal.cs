namespace Medibook.Data.Repositories
{
    using MediBook.Core.Models;
    using Medibook.Data.DataAccess;

    /// <summary>
    /// PatientNoteDal
    /// </summary>
    public class PatientNoteDal : RepositoryBase<PatientNote>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientNoteDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public PatientNoteDal(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}