namespace MediBook.Data.Repositories
{
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.Extensions.Logging;

    public class PatientUserDal : RepositoryBase<PatientUser>, IPatientUserDal
    {
        public PatientUserDal(IDatabaseContext context, ILogger<PatientUserDal> logger)
            : base (context, logger)
        {
        }
    }
}