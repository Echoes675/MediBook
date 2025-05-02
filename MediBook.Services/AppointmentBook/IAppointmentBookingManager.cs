namespace MediBook.Services.AppointmentBook
{
    using System.Threading.Tasks;
    using MediBook.Core.Models;

    public interface IAppointmentBookingManager
    {
        /// <summary>
        /// Gets the Appointment history for a given patient where the calling user is a MedicalPractitioner and was
        /// also associated with the appointment
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AppointmentBookResults> GetPatientAppointmentHistory(int patientId, int userId);

        /// <summary>
        /// Book a new appointment for a patient
        /// </summary>
        /// <returns></returns>
        Task<AppointmentBookResults> BookAppointmentAsync(AddOrUpdateAppointmentData data);

        /// <summary>
        /// Update an existing appointment for a patient
        /// </summary>
        /// <returns></returns>
        Task<AppointmentBookResults> UpdateAppointmentAsync(AddOrUpdateAppointmentData data);

        /// <summary>
        /// Cancel a booked appointment for a patient
        /// </summary>
        /// <returns></returns>
        Task<AppointmentBookResults> CancelAppointmentAsync(int slotId);

        /// <summary>
        /// Cancel a booked appointment for a patient
        /// </summary>
        /// <returns></returns>
        Task<AppointmentBookResults> CancelAppointmentAsync(AppointmentSlot slot);

        /// <summary>
        /// Returns a list of SelectListItems representing the appointment slots that
        /// are available to book for a given active Medical Practitioner from today (now) onwards
        /// </summary>
        /// <returns></returns>
        Task<AppointmentBookResults> GetMedicalPractitionerFreeSlotsSelectList(int slotId);
    }
}