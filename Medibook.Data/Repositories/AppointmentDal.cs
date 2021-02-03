namespace MediBook.Data.Repositories
{
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The AppointmentDal
    /// </summary>
    public class AppointmentDal : RepositoryBase<Appointment>, IAppointmentDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public AppointmentDal(IDatabaseContext databaseContext, ILogger<AppointmentDal> logger) : base(databaseContext, logger)
        {
        }
    }
}