namespace Medibook.Data.Repositories
{
    using MediBook.Core.Models;
    using Medibook.Data.DataAccess;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The UserDal
    /// </summary>
    public class UserDal : RepositoryBase<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        /// <param name="logger"></param>
        public UserDal(IDatabaseContext databaseContext, ILogger<UserDal> logger) : base(databaseContext, logger)
        {
        }
    }
}