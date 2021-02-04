namespace MediBook.Core.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;

    /// <summary>
    /// A summary of the Appointment details
    /// </summary>
    public class AppointmentDetails
    {
        /// <summary>
        /// Initializes an instance of the <see cref="AppointmentDetails"/> class
        /// </summary>
        public AppointmentDetails()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="AppointmentDetails"/> class
        /// </summary>
        public AppointmentDetails(User medicalPractitioner, AppointmentSlot appointmentSlot)
        {
            if (medicalPractitioner == null)
            {
                throw new ArgumentNullException(nameof(medicalPractitioner));
            }

            if (appointmentSlot == null)
            {
                throw new ArgumentNullException(nameof(appointmentSlot));
            }

            AppointmentId = appointmentSlot.AppointmentId;
            State = appointmentSlot.State;
            AppointmentDateTime = appointmentSlot.AppointmentDateTime;
            AppointmentDurationInMins = appointmentSlot.AppointmentDurationInMins;
            AppointmentSlotId = appointmentSlot.Id;
            
            
            if (appointmentSlot.Appointment != null)
            {
                PatientId = appointmentSlot.Appointment.PatientId;
                PatientFirstname = appointmentSlot?.Appointment?.Patient?.Firstname;
                PatientLastname = appointmentSlot?.Appointment?.Patient?.Lastname;
            }

            if (medicalPractitioner.JobDescription != null & medicalPractitioner.EmployeeDetails != null)
            {
                MedicalPractitionerDetails = new MedicalPractitionerDetails(
                    medicalPractitioner.Id,
                    medicalPractitioner.JobDescription.Description,
                    medicalPractitioner.EmployeeDetails);
            }
        }

        /// <summary>
        /// The Appointment Id
        /// </summary>
        public int AppointmentId { get; set; }

        /// <summary>
        /// The Appointment State
        /// </summary>
        public SlotState State { get; set; }

        /// <summary>
        /// The day and time of the appointment
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime AppointmentDateTime { get; set; }

        /// <summary>
        /// The Appointment duration in minutes
        /// </summary>
        public int AppointmentDurationInMins { get; set; }

        /// <summary>
        /// The Id of the Appointment Slot
        /// </summary>
        public int AppointmentSlotId { get; set; }

        /// <summary>
        /// The Id of the Patient
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The Patient's firstname
        /// </summary>
        public string PatientFirstname { get; set; }

        /// <summary>
        /// The Patient's lastname
        /// </summary>
        public string PatientLastname { get; set; }

        /// <summary>
        /// Details of the associated Medical Practitioner
        /// </summary>
        public MedicalPractitionerDetails MedicalPractitionerDetails { get; set; }
    }
}