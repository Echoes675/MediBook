namespace MediBook.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using MediBook.Core.Enums;

    /// <summary>
    /// The Appointment slot
    /// </summary>
    public class AppointmentSlot : IDbEntity
    {
        /// <summary>
        /// The Appointment Slot Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Appointment State
        /// </summary>
        [Required]
        public SlotState State { get; set; }

        /// <summary>
        /// The day and time of the appointment
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime AppointmentDateTime { get; set; }

        /// <summary>
        /// The Appointment duration in minutes
        /// </summary>
        [Required]
        public int AppointmentDurationInMins { get; set; }

        ///// <summary>
        ///// The Id of the booked Appointment
        ///// </summary>
        //public int AppointmentId { get; set; }

        ///// <summary>
        ///// The navigation property for the booked appointment
        ///// </summary>
        //public Appointment Appointment { get; set; }

        /// <summary>
        /// The Appointment state
        /// </summary>
        public AppointmentState AppointmentState { get; set; }

        /// <summary>
        /// The Patient Id
        /// </summary>
        public int? PatientId { get; set; }

        /// <summary>
        /// The Patient navigation property
        /// </summary>
        public Patient Patient { get; set; }
    }
}