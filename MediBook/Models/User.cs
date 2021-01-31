namespace MediBook.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using MediBook.Core.Enums;

    /// <summary>
    /// The UserAccount class
    /// </summary>
    public class User : IDbEntity
    {
        /// <summary>
        /// The entity's Id in the Users table
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
        /// The user's role
        /// </summary>
        [Required]
        public UserRole Role { get; set; }

        /// <summary>
        /// The Account State
        /// </summary>
        [Required]
        public AccountState State { get; set; }
    }
}