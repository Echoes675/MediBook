namespace MediBook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.DTOs;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The AppointmentSessionDal
    /// </summary>
    public class AppointmentSessionDal : RepositoryBase<AppointmentSession>, IAppointmentSessionDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentSessionDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public AppointmentSessionDal(IDatabaseContext databaseContext, ILogger<AppointmentSessionDal> logger) : base(databaseContext, logger)
        {
        }

        /// <summary>
        /// Returns all Appointment Sessions including associated Appointment Slots and Appointments
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<AppointmentSession>> GetAllAsync()
        {
            var retVal = await Db.Set<AppointmentSession>()
                .Include(m => m.MedicalPractitioner)
                .Include(x => x.AppointmentSlots)
                .ToListAsync().ConfigureAwait(false);
            _log.LogDebug($"All Entities returned of type. \"EntityType\"={typeof(AppointmentSession)}");
            return retVal;
        }

        /// <summary>
        /// Returns an entity of a given type using its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<AppointmentSession> GetEntityAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var retVal = await Db.Set<AppointmentSession>()
                .Include(m => m.MedicalPractitioner)
                .Include(x => x.AppointmentSlots)
                .ThenInclude(p => p.Patient)
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);
            _log.LogDebug($"All Entities returned of type. \"EntityType\"={typeof(AppointmentSession)}");
            return retVal;
        }

        /// <summary>
        /// Returns the Session that owns the slot
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        public Task<AppointmentSession> GetSessionThatOwnsSlot(int slotId)
        {
            return Db.Set<AppointmentSession>().Where(x => x.AppointmentSlots.Any(y => y.Id == slotId)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Filters and orders the list of Appointment Sessions per the requirements. Includes Appointment Slots and Appointments
        /// </summary>
        /// <param name="appointmentSessionWhere"></param>
        /// <param name="appointmentSessionOrderBy"></param>
        /// <returns></returns>
        public async Task<List<AppointmentSession>> FilterAndOrderAsync(Func<AppointmentSession, bool> appointmentSessionWhere, Func<AppointmentSession, DateTime> appointmentSessionOrderBy)
        {
            if (appointmentSessionWhere == null)
            {
                throw new ArgumentNullException(nameof(appointmentSessionWhere));
            }

            if (appointmentSessionOrderBy == null)
            {
                throw new ArgumentNullException(nameof(appointmentSessionOrderBy));
            }

            return await Task.Run(() => (Db.Set<AppointmentSession>()
                    .Include(m => m.MedicalPractitioner)
                    .Include(x => x.AppointmentSlots)
                .ThenInclude(p => p.Patient)
                .AsEnumerable() ?? throw new InvalidOperationException(nameof(AppointmentSession)))
                .Where(appointmentSessionWhere).OrderBy(appointmentSessionOrderBy).ToList());
        }

        /// <summary>
        /// Verifies whether an Appointment session already exists in the time block for a given Medical Practitioner
        /// </summary>
        /// <param name="medicalPractitionerId"></param>
        /// <param name="sessionStartTime"></param>
        /// <param name="durationInMins"></param>
        /// <returns></returns>
        public async Task<bool> CheckEntityExistsAsync(int medicalPractitionerId, DateTime sessionStartTime, int durationInMins)
        {
            var checkTask = Task.Run(() =>
             Filter(existing =>
                // Filter to a given medical practitioner
                existing.MedicalPractitionerId == medicalPractitionerId &&
                // Check for existing sessions whose start time is before the new session start time and whose end time is after the new session's start time
                (existing.StartDateTime < sessionStartTime && existing.StartDateTime.AddMinutes(existing.DurationInMins) > sessionStartTime) ||
                // OR the new session start time is before any existing sessions start and whose end time is after any existing session's start time
                (sessionStartTime < existing.StartDateTime && sessionStartTime.AddMinutes(durationInMins) > existing.StartDateTime) ||
                // OR where any existing session's start time starts after the new session start time but finishes before the new session is due to end
                (existing.StartDateTime > sessionStartTime && existing.StartDateTime.AddMinutes(existing.DurationInMins) < sessionStartTime) ||
                // OR where the new session starts after any existing session's start time but finishes before the existing session is due to end
                (sessionStartTime > existing.StartDateTime && sessionStartTime.AddMinutes(durationInMins) < existing.StartDateTime))
            );

            var result = await checkTask;

            return result != null;
        }

        /// <summary>
        /// Verifies whether an Appointment session already exists in the time block for a given Medical Practitioner
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CheckEntityExists(AppointmentSession entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return await CheckEntityExistsAsync(entity.MedicalPractitionerId, entity.StartDateTime, entity.DurationInMins);
        }

        /// <summary>
        /// Returns a list of a patient's appointment slots filtered to the calling Medical Practitioner's Id
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppointmentSlot>> GetPatientAppointmentSlotsAssociatedWithMedicalPractitionerSessions(int userId, int patientId)
        {
            var appointmentSessions = Db.Set<AppointmentSession>().Where(x => x.MedicalPractitionerId == userId).Include(m => m.MedicalPractitioner)
                .Include(x => x.AppointmentSlots)
                .ThenInclude(p => p.Patient);
            
            var slots =  await appointmentSessions.SelectMany(x =>
                x.AppointmentSlots.Where(y => y.PatientId == patientId)).ToListAsync();

            return slots;
        }
    }
}