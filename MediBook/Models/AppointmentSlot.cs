namespace MediBook.Core.Models
{
    /// <summary>
    /// The Appointment Slot
    /// </summary>
    public class AppointmentSlot : IDbEntity
    {
        /// <summary>
        /// The AppointmentSlot Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Appointment Id
        /// </summary>
        public int AppointmentId { get; set; }

        /// <summary>
        /// The Appointment Navigation property
        /// </summary>
        public Appointment Appointment { get; set; }

        /// <summary>
        /// The AppointmentSession Id
        /// </summary>
        public int AppointmentSessionId { get; set; }

        /// <summary>
        /// The AppointmentSession Navigation property
        /// </summary>
        public AppointmentSession AppointmentSession { get; set; }
    }
}
