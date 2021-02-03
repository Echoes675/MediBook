namespace MediBook.Services.AppointmentBook
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The AppointmentSession Configuration
    /// </summary>
    public class AppointmentSessionConfiguration
    {
        /// <summary>
        /// The duration of the AppointmentSession in minutes
        /// </summary>
        [Required]
        public int DurationInMins { get; set; }

        /// <summary>
        /// The timestamp of when the AppointmentSession begins
        /// </summary>
        [Required]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// The number of appointment slots
        /// </summary>
        [Required]
        public int NumberOfAppointmentSlots { get; set; }

        /// <summary>
        /// The Id of the associated Medical Practitioner Employee account
        /// </summary>
        public int MedicalPractitionerId { get; set; }
    }
}