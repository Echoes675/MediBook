namespace Medibook.Data.Repositories
{
    using MediBook.Core.Models;
    using Medibook.Data.DataAccess;

    /// <summary>
    /// The UserDal
    /// </summary>
    public class UserDal : RepositoryBase<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public UserDal(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}