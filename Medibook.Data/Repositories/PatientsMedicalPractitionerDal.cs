namespace MediBook.Data.Repositories
{
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The PatientsMedicalPractitionerDal
    /// </summary>
    public class PatientsMedicalPractitionerDal : RepositoryBase<PatientsMedicalPractitioner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientsMedicalPractitionerDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public PatientsMedicalPractitionerDal(IDatabaseContext databaseContext, ILogger<PatientsMedicalPractitionerDal> logger) : base(databaseContext, logger)
        {
        }
    }
}