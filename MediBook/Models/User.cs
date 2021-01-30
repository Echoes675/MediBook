namespace MediBook.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MediBook.Core.Enums;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The User account
    /// </summary>
    [Index(nameof(JobDescription))]
    [Index(nameof(Role))]
    public class User : IDbEntity
    {
        /// <summary>
        /// The User Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Username
        /// </summary>
        [Required, MaxLength(50), MinLength(3)]
        public string Username { get; set; }

        /// <summary>
        /// The PasswordHash
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// The PasswordSalt
        /// </summary>
        public byte[] PasswordSalt { get; set; }

        /// <summary>
        /// The user's title
        /// </summary>
        [Required]
        public Title Title { get; set; }

        /// <summary>
        /// The user's firstname
        /// </summary>
        [Required, MaxLength(50), MinLength(2)]
        public string Firstname { get; set; }

        /// <summary>
        /// The user's lastname
        /// </summary>
        [Required, MaxLength(50), MinLength(2)]
        public string Lastname { get; set; }

        /// <summary>
        /// The Id of the user's Job Description
        /// </summary>
        public int JobDescriptionId { get; set; }

        /// <summary>
        /// The navigation property for the user's Job Description
        /// </summary>
        [Required]
        public JobDescription JobDescription { get; set; }

        /// <summary>
        /// The user's role
        /// </summary>
        [Required]
        public UserRole Role { get; set; }

        /// <summary>
        /// The Account State
        /// </summary>
        [Required]
        public AccountState State { get; set; }

        /// <summary>
        /// Navigation property for any patients registered with this user
        /// </summary>
        public ICollection<Patient> Patients { get; set; }

        /// <summary>
        /// Navigation property for patient notes this user has made
        /// </summary>
        public ICollection<PatientNote> PatientNotes { get; set; }
    }
}