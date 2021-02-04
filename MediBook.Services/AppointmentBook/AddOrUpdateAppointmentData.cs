namespace MediBook.Services.AppointmentBook
{
    /// <summary>
    /// Data required to book a new appointment or update an existing one
    /// </summary>
    public class AddOrUpdateAppointmentData
    {
        /// <summary>
        /// The patient Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The Id of the selected slot
        /// </summary>
        public int SlotId { get; set; }

        /// <summary>
        /// The Medical Practitioner Id
        /// </summary>
        public int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The Appointment Id used for the update process
        /// </summary>
        public int AppointmentId { get; set; }
    }
}