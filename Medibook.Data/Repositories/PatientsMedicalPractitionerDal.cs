namespace MediBook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The PatientsMedicalPractitionerDal
    /// </summary>
    public class PatientsMedicalPractitionerDal : RepositoryBase<PatientsMedicalPractitioner>, IPatientsMedicalPractitionerDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientsMedicalPractitionerDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public PatientsMedicalPractitionerDal(IDatabaseContext databaseContext, ILogger<PatientsMedicalPractitionerDal> logger) 
            : base(databaseContext, logger)
        {
        }

        /// <summary>
        /// Filter entities in a DbSet based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.InvalidOperationException">Condition.</exception>
        public Task<IEnumerable<PatientsMedicalPractitioner>> FilterAsync(Func<PatientsMedicalPractitioner, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return Task.Run(() => Filter(predicate));
        }

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        public override Task<bool> CheckEntityExistsAsync(PatientsMedicalPractitioner entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return CheckEntityExistsAsync(entity.PatientId, entity.MedicalPractitionerId);
        }

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="medicalPractitionerId"></param>
        /// <returns></returns>
        public async Task<bool> CheckEntityExistsAsync(int patientId, int medicalPractitionerId)
        {
            if (patientId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(patientId));
            }

            if (medicalPractitionerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(medicalPractitionerId));
            }

            var foundEntities = await FilterAsync(x =>
                x.MedicalPractitionerId == medicalPractitionerId && x.PatientId == patientId);
            return foundEntities.Any();
        }
    }
}