namespace MediBook.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// The Create AppointmentSession ViewModel
    /// </summary>
    public class CreateAppointmentSessionViewModel
    {
        /// <summary>
        /// The list of select list items representing the active MedicalPractitioners
        /// </summary>
        public List<SelectListItem> MedicalPractitioners { get; set; }

        /// <summary>
        /// The MedicalPractitioner Id
        /// </summary>
        [Required (ErrorMessage = "Please select a Medical Practitioner")]
        public int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The date of the AppointmentSession
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /// <summary>
        /// The time of the AppointmentSession
        /// </summary>
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The duration in Minutes
        /// </summary>
        [Required]
        public int DurationInMins { get; set; }

        /// <summary>
        /// The number of AppointmentSlots
        /// </summary>
        [Required]
        [Range(1,120)]
        public int NumberOfAppointmentSlots { get; set; }
    }
}