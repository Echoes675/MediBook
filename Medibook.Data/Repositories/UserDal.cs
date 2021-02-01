namespace MediBook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The UserDal
    /// </summary>
    public class UserDal : RepositoryBase<User>, IUserDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        /// <param name="logger"></param>
        public UserDal(IDatabaseContext databaseContext, ILogger<UserDal> logger) : base(databaseContext, logger)
        {
        }

        /// <summary>
        /// Searches for any registered users using the supplied username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            return await Db.Set<User>().Include(x => x.JobDescription).FirstOrDefaultAsync(x => 
                x.Username.ToLower().CompareTo(username.Trim().ToLower()) == 0).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the Users full account and employee details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserFullDetailsAsync(int id)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), $"Id cannot be less than 1. \"Id\"={id}");
            }

            return await Db.Set<User>()
                .Include(x => x.JobDescription)
                .Include(x => x.EmployeeDetails)
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Returns all the entities in a particular DbSet
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            var retVal = await Db.Set<User>().Include(x => x.JobDescription).ToListAsync().ConfigureAwait(false);
            _log.LogDebug($"All Entities returned of type. \"EntityType\"={typeof(User)}");
            return retVal;
        }

        /// <summary>
        /// Filter entities in a DbSet based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.InvalidOperationException">Condition.</exception>
        public override IEnumerable<User> Filter(Func<User, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (Db.Set<User>().Include(x => x.JobDescription).AsEnumerable() ?? throw new InvalidOperationException(nameof(User))).Where(predicate);
        }

        /// <summary>
        /// Method to soft delete a User from the database if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<bool> DeleteAsync(int id)
        {
            if (id < 1)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var entity = await GetEntityAsync(id).ConfigureAwait(false);
            if (entity != null)
            {
                entity.State = AccountState.Deleted;
                await DbUpdateAsync(entity).ConfigureAwait(false);
                _log.LogDebug($"Entity marked as deleted in the db. \"Id\"={entity.Id} \"Username\"={entity.Username}");
                return true;
            }

            _log.LogWarning($"Entity cannot be deleted as it does not exist in the db. \"Id\"={id}");
            return false;
        }
    }
}