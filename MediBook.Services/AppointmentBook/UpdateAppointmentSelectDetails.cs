namespace MediBook.Services.AppointmentBook
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UpdateAppointmentSelectDetails
    {
        /// <summary>
        /// The Patient ID
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The Slot Id
        /// </summary>
        public int SlotId { get; set; }

        /// <summary>
        /// MedicalPractitionerId
        /// </summary>
        public int MedicalPractitionerId { get; set; }

        /// <summary>
        /// The FreeSlotsSelectListItems
        /// </summary>
        public List<SelectListItem> FreeSlotsSelectListItems { get; set; }
    }
}