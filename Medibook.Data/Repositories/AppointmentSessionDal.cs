namespace Medibook.Data.Repositories
{
    using MediBook.Core.Models;
    using Medibook.Data.DataAccess;

    /// <summary>
    /// The AppointmentSessionDal
    /// </summary>
    public class AppointmentSessionDal : RepositoryBase<AppointmentSession>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentSessionDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public AppointmentSessionDal(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}