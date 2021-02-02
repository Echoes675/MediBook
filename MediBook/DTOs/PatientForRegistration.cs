namespace MediBook.Core.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using MediBook.Core.Enums;

    /// <summary>
    /// The Patient details for registration
    /// </summary>
    public class PatientForRegistration
    {
        /// <summary>
        /// The Patient's Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The patient's title
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        public Title Title { get; set; }

        /// <summary>
        /// The patient's firstname
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "Last name must be less than 50 characters")]
        public string Firstname { get; set; }

        /// <summary>
        /// The patient's lastname
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Last name must be less than 50 characters")]
        public string Lastname { get; set; }

        /// <summary>
        /// The patient's date of birth
        /// </summary>
        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// The patient's Health and Care number
        /// </summary>
        [MaxLength(10, ErrorMessage = "Health and Care number is 10 digits in length")]
        [MinLength(10, ErrorMessage = "Health and Care number is 10 digits in length")]
        public long HealthAndCare { get; set; }

        /// <summary>
        /// The first line of the patient's address
        /// </summary>
        [Required (ErrorMessage = "First Line of Address is required")]
        [MaxLength(50, ErrorMessage = "First line of Address must be less than 50 characters")]
        public string Address1 { get; set; }

        /// <summary>
        /// The second line of the patient's address
        /// </summary>
        [MaxLength(50, ErrorMessage = "Second line of Address must be less than 50 characters")]
        public string Address2 { get; set; }

        /// <summary>
        /// The city of the patient's address
        /// </summary>
        [MaxLength(50, ErrorMessage = "City must be less than 50 characters")]
        public string City { get; set; }

        /// <summary>
        /// The county of the patient's address
        /// </summary>
        [MaxLength(50, ErrorMessage = "County must be less than 50 characters")]
        public string County { get; set; }

        /// <summary>
        /// The patient's post code
        /// </summary>
        [Required(ErrorMessage = "Post Code is required")]
        [DataType(DataType.PostalCode, ErrorMessage = "Invalid Post Code format")]
        public string PostCode { get; set; }

        /// <summary>
        /// The patient's phone number
        /// </summary>
        [MaxLength(20, ErrorMessage = "Phone number must be less than 20 digits")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The patient's mobile phone number
        /// </summary>
        [MaxLength(20, ErrorMessage = "Mobile phone number must be less than 20 digits")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number format")]
        public string MobilePhone { get; set; }

        /// <summary>
        /// The patient's email address
        /// </summary>
        [MaxLength(50, ErrorMessage = "Email must be less than 50 characters")]
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        /// <summary>
        /// The patient's status
        /// </summary>
        [Required(ErrorMessage = "The Patient's status is required")]
        public PatientStatus Status { get; set; }
    }
}