namespace MediBook.Data.Repositories
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The UserDal
    /// </summary>
    public class UserDal : RepositoryBase<Employee>, IUserDal
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
            return await Db.Set<User>().FirstOrDefaultAsync(x => x.Username.ToLower().CompareTo(username.Trim().ToLower()) == 0).ConfigureAwait(false);
        }
    }
}