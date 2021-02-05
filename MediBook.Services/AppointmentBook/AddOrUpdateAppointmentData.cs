namespace MediBook.Services.AppointmentBook
{
    using System.Collections.Generic;
    using MediBook.Core.DTOs;
    using Microsoft.AspNetCore.Mvc.Rendering;

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
        /// The Id of the newly chosen slot
        /// </summary>
        public int NewSlotId { get; set; }

        /// <summary>
        /// The Medical Practitioner Id
        /// </summary>
        public int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The Appointment Id used for the update process
        /// </summary>
        public int AppointmentId { get; set; }

        /// <summary>
        /// Appointment sessions showing only the available slots
        /// </summary>
        public List<AppointmentSessionDetails> SessionsWithFreeSlots { get; set; }

        /// <summary>
        /// The SelectListItems
        /// </summary>
        public List<SelectListItem> SelectList { get; set; }
    }
}