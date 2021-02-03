namespace MediBook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The AppointmentSlotDal
    /// </summary>
    public class AppointmentSlotDal : RepositoryBase<AppointmentSlot>, IAppointmentSlotDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentSlotDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public AppointmentSlotDal(IDatabaseContext databaseContext, ILogger<AppointmentSlotDal> logger) : base(databaseContext, logger)
        {
        }

        /// <summary>
        /// Returns all the entities in a particular DbSet
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<AppointmentSlot>> GetAllAsync()
        {
            var retVal = await Db.Set<AppointmentSlot>().Include(x => x.Appointment).ToListAsync().ConfigureAwait(false);
            _log.LogDebug($"All Entities returned of type. \"EntityType\"={typeof(AppointmentSlot)}");
            return retVal;
        }

        /// <summary>
        /// Filter entities in a DbSet based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.InvalidOperationException">Condition.</exception>
        public override IEnumerable<AppointmentSlot> Filter(Func<AppointmentSlot, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (Db.Set<AppointmentSlot>().Include(x => x.Appointment).AsEnumerable() ??
                    throw new InvalidOperationException(nameof(AppointmentSlot))).Where(predicate);
        }
    }
}