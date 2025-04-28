namespace MediBook.Data.Repositories
{
    using System;
    using System.Linq;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class PatientUserDal : RepositoryBase<PatientUser>, IPatientUserDal
    {
        public PatientUserDal(IDatabaseContext context, ILogger<PatientUserDal> logger)
            : base (context, logger)
        {
        }

        /// <summary>
        /// Gets the Patient associated with the User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Patient GetAssociatedPatient(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            var patientUsers = Db.Set<PatientUser>().Include(x => x.PatientDetails).ToList();

            var patients = patientUsers.Select(x => x.PatientDetails);

            var patient = patients.FirstOrDefault();

            return patient;
        }
    }
}