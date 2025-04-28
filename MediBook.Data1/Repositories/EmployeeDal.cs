namespace MediBook.Data.Repositories
{
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The EmployeeDal
    /// </summary>
    public class EmployeeDal : RepositoryBase<Employee>, IEmployeeDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        /// <param name="logger"></param>
        public EmployeeDal(IDatabaseContext databaseContext, ILogger<EmployeeDal> logger) : base(databaseContext, logger)
        {
        }
    }
}