namespace Medibook.Data.Repositories
{
    using MediBook.Core.Models;
    using Medibook.Data.DataAccess;

    /// <summary>
    /// The PatientsMedicalPractitionerDal
    /// </summary>
    public class PatientsMedicalPractitionerDal : RepositoryBase<PatientsMedicalPractitioner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientsMedicalPractitionerDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public PatientsMedicalPractitionerDal(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}