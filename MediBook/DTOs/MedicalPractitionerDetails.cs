namespace MediBook.Core.DTOs
{
    using System;
    using MediBook.Core.Models;

    /// <summary>
    /// The MedicalPractitioner Details
    /// </summary>
    public class MedicalPractitionerDetails
    {
        /// <summary>
        /// Initializes an instance of <see cref="MedicalPractitionerDetails"/> class
        /// </summary>
        public MedicalPractitionerDetails()
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="MedicalPractitionerDetails"/> class
        /// </summary>
        public MedicalPractitionerDetails(int medicalPractitionerUserId, string jobDescription, Employee medicalPractitionerEmployeeDetails)
        {
            if (medicalPractitionerEmployeeDetails == null)
            {
                throw new ArgumentNullException(nameof(medicalPractitionerEmployeeDetails));
            }

            Id = medicalPractitionerUserId > 0 ? medicalPractitionerUserId :
                throw new ArgumentOutOfRangeException(nameof(medicalPractitionerUserId));

            FirstName = string.IsNullOrEmpty(medicalPractitionerEmployeeDetails.Firstname) ?
                throw new ArgumentNullException(nameof(medicalPractitionerEmployeeDetails.Firstname)) :
                medicalPractitionerEmployeeDetails.Firstname;

            LastName = string.IsNullOrEmpty(medicalPractitionerEmployeeDetails.Lastname) ?
                throw new ArgumentNullException(nameof(medicalPractitionerEmployeeDetails.Lastname)) :
                medicalPractitionerEmployeeDetails.Lastname;

            JobDescription = string.IsNullOrEmpty(jobDescription) ? 
                throw new ArgumentNullException(nameof(jobDescription)) : jobDescription;
        }

        /// <summary>
        /// The Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The MedicalPractitioner's Firstname
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The MedicalPractitioner's Lastname
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The MedicalPractitioner's JobDescription
        /// </summary>
        public string JobDescription { get; set; }
    }
}