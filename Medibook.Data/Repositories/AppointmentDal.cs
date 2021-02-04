namespace MediBook.Data.Repositories
{
    using System.Threading.Tasks;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Method to soft delete an Appointment if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<bool> DeleteAsync(int id)
        {
            var appointment = await Db.Set<Appointment>().FirstOrDefaultAsync(e => e.Id == id);
            if (appointment == null)
            {
                return false;
            }

            // Set the state to Cancelled to indicate the deleted Appointment
            appointment.State = AppointmentState.Cancelled;

            Db.Set<Appointment>().Update(appointment);
            await Db.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
    }
}