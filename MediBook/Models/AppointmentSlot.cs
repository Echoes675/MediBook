namespace MediBook.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The Appointment Slot
    /// </summary>
    [Index(nameof(AppointmentSessionId))]
    [Index(nameof(AppointmentId))]
    public class AppointmentSlot : IDbEntity
    {
        /// <summary>
        /// The AppointmentSlot Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Appointment Id
        /// </summary>
        [ForeignKey("AppointmentId")]
        public int AppointmentId { get; set; }

        /// <summary>
        /// The Appointment Navigation property
        /// </summary>
        [Required]
        public Appointment Appointment { get; set; }

        /// <summary>
        /// The AppointmentSession Id
        /// </summary>
        public int AppointmentSessionId { get; set; }

        /// <summary>
        /// The AppointmentSession Navigation property
        /// </summary>
        [Required]
        public AppointmentSession AppointmentSession { get; set; }
    }
}
