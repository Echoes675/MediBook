namespace MediBook.Data.Repositories
{
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The AppointmentSessionDal
    /// </summary>
    public class AppointmentSessionDal : RepositoryBase<AppointmentSession>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentSessionDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public AppointmentSessionDal(IDatabaseContext databaseContext, ILogger<AppointmentSessionDal> logger) : base(databaseContext, logger)
        {
        }
    }
}