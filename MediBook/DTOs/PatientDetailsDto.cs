namespace MediBook.Core.DTOs
{
    using System;
    using System.Collections.Generic;
    using MediBook.Core.Enums;
    using MediBook.Core.Models;

    /// <summary>
    /// The Patient's details
    /// </summary>
    public class PatientDetailsDto
    {
        /// <summary>
        /// Initializes an instance of the <see cref="PatientDetailsDto"/> class
        /// </summary>
        public PatientDetailsDto()
        {
            
        }

        /// <summary>
        /// Initializes an instance of the <see cref="PatientDetailsDto"/> class
        /// </summary>
        public PatientDetailsDto(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            Id = patient.Id;
            Title = patient.Title;
            Firstname = patient.Firstname;
            Lastname = patient.Lastname;
            DateOfBirth = patient.DateOfBirth;
            HealthAndCare = patient.HealthAndCare;
            Address1 = patient.Address1;
            Address2 = patient.Address2;
            City = patient.City;
            County = patient.County;
            PostCode = patient.PostCode;
            PhoneNumber = patient.PhoneNumber;
            MobilePhone = patient.MobilePhone;
            Email = patient.Email;
            PatientNotes = patient.PatientNotes;
            Appointments = patient.Appointments;
            PatientStatus = patient.Status;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="PatientDetailsDto"/> class
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public PatientDetailsDto(Patient patient, List<MedicalPractitionerDetails> medicalPractitioners)
        : this (patient)
        {
            MedicalPractitioners = medicalPractitioners ?? throw new ArgumentNullException(nameof(medicalPractitioners));
        }

        /// <summary>
        /// The Patient's Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The patient's title
        /// </summary>
        public Title Title { get; set; }

        /// <summary>
        /// The patient's firstname
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// The patient's lastname
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// The patient's date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// The patient's Health and Care number
        /// </summary>
        public long HealthAndCare { get; set; }

        /// <summary>
        /// The first line of the patient's address
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// The second line of the patient's address
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// The city of the patient's address
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The county of the patient's address
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// The patient's post code
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// The patient's phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The patient's mobile phone number
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// The patient's email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The patient's status
        /// </summary>
        public PatientStatus PatientStatus { get; set; }

        /// <summary>
        /// The MedicalPractitioners that the Patient is registered with
        /// </summary>
        public List<MedicalPractitionerDetails> MedicalPractitioners { get; set; }

        /// <summary>
        /// The patient's notes
        /// </summary>
        public ICollection<PatientNote> PatientNotes { get; set; }

        /// <summary>
        /// The patient's appointments
        /// </summary>
        public ICollection<Appointment> Appointments { get; set; }
    }
}