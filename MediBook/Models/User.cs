namespace MediBook.Core.Models
{
    using System.Collections.Generic;
    using MediBook.Core.Enums;

    /// <summary>
    /// The User account
    /// </summary>
    public class User : IDbEntity
    {
        /// <summary>
        /// The User Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Username
        /// </summary>
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
        public Title Title { get; set; }

        /// <summary>
        /// The user's firstname
        /// </summary>
        public string Firstname { get; set; }
        
        /// <summary>
        /// The user's lastname
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// The Id of the user's Job Description
        /// </summary>
        public int JobDescriptionId { get; set; }

        /// <summary>
        /// The navigation property for the user's Job Description
        /// </summary>
        public JobDescription JobDescription { get; set; }

        /// <summary>
        /// The user's role
        /// </summary>
        public UserRole Role { get; set; }

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