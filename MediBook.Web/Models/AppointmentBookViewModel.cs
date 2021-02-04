namespace MediBook.Web.Models
{
    using System;
    using System.Collections.Generic;
    using MediBook.Core.DTOs;

    /// <summary>
    /// The AppointmentBook View Model
    /// </summary>
    public class AppointmentBookViewModel
    {
        /// <summary>
        /// The date of the presented session(s)
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// The appointment sessions
        /// </summary>
        public List<AppointmentSessionDetails> AppointmentSessions { get; set; }
    }
}