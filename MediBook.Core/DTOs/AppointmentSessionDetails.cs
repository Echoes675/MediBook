namespace MediBook.Core.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MediBook.Core.Models;

    /// <summary>
    /// The Appointment Session details
    /// </summary>
    public class AppointmentSessionDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentSessionDetails"/> class
        /// </summary>
        public AppointmentSessionDetails()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentSessionDetails"/> class
        /// </summary>
        public AppointmentSessionDetails(AppointmentSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentSessionDetails"/> class
        /// </summary>
        public AppointmentSessionDetails(AppointmentSession session, User medicalPractitioner)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (medicalPractitioner == null)
            {
                throw new ArgumentNullException(nameof(medicalPractitioner));
            }

            Id = session.Id;
            MedicalPractitioner = new MedicalPractitionerDetails(medicalPractitioner.Id, medicalPractitioner.JobDescription.Description, medicalPractitioner.EmployeeDetails);
            StartDateTime = session.StartDateTime;
            EndDateTime = session.StartDateTime.AddMinutes(session.DurationInMins);
            AppointmentSlots = session.AppointmentSlots.ToList();
        }

        /// <summary>
        /// The Id of the appointment session
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Details of the Medical practitioner this session is assigned to
        /// </summary>
        public MedicalPractitionerDetails MedicalPractitioner { get; set; }

        /// <summary>
        /// The DateTime of the start of this session
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// The DateTime of the end of this session
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// The Appointment Slots
        /// </summary>
        public List<AppointmentSlot> AppointmentSlots { get; set; }
    }
}