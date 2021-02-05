namespace MediBook.Services.AppointmentBook
{
    using System.Collections.Generic;
    using MediBook.Core.DTOs;
    using MediBook.Services.Enums;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// The results AppointmentBook operations
    /// </summary>
    public class AppointmentBookResults
    {
        /// <summary>
        /// The result code
        /// </summary>
        public ServiceResultStatusCode ResultCode { get; set; }

        /// <summary>
        /// The Appointment Sessions details
        /// </summary>
        public List<AppointmentSessionDetails> AppointmentSessionDetails { get; set; }

        /// <summary>
        /// The Appointments Details
        /// </summary>
        public List<AppointmentDetails> AppointmentsDetails { get; set; }

        /// <summary>
        /// The FreeSlotsSelectListItems
        /// </summary>
        public List<SelectListItem> SelectList { get; set; }
    }
}