namespace Medibook.Data.Repositories
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using Medibook.Data.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The JobDescriptionDal
    /// </summary>
    public class JobDescriptionDal : RepositoryBase<JobDescription>, IJobDescriptionDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobDescriptionDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        /// <param name="logger"></param>
        public JobDescriptionDal(IDatabaseContext databaseContext, ILogger<JobDescriptionDal> logger) : base(databaseContext, logger)
        {
        }

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        public override async Task<bool> CheckEntityExistsAsync(JobDescription entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (string.IsNullOrEmpty(entity.Description))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var dbEntity = await Db.Set<JobDescription>()
                .FirstOrDefaultAsync(x => 
                    string.Compare(x.Description, entity.Description, StringComparison.InvariantCultureIgnoreCase) == 0);

            return dbEntity != null;
        }
    }
}