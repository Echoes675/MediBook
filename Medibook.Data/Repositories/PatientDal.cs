namespace Medibook.Data.Repositories
{
    using MediBook.Core.Models;
    using Medibook.Data.DataAccess;

    /// <summary>
    /// The PatientDal
    /// </summary>
    public class PatientDal : RepositoryBase<Patient>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public PatientDal(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}