namespace Medibook.Data.Repositories
{
    using MediBook.Core.Models;
    using Medibook.Data.DataAccess;

    /// <summary>
    /// The AppointmentDal
    /// </summary>
    public class AppointmentDal : RepositoryBase<Appointment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public AppointmentDal(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}