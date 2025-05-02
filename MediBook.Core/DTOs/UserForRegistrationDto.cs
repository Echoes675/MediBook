namespace MediBook.Core.DTOs
{
    using System.ComponentModel.DataAnnotations;
    using MediBook.Core.Enums;

    /// <summary>
    /// DTO for user registration details
    /// </summary>
    public class UserForRegistrationDto
    {
        /// <summary>
        /// The user's title
        /// </summary>
        [Required]
        public Title Title { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        [Required, MaxLength(50), MinLength(2)]
        public string FirstName { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        [Required, MaxLength(50), MinLength(2)]
        public string LastName { get; set; }

        /// <summary>
        /// The username
        /// </summary>
        [Required, MaxLength(50), MinLength(3)]
        public string Username { get; set; }

        /// <summary>
        /// The initial password
        /// </summary>
        [Required, MinLength(10)]
        public string Password { get; set; }

        /// <summary>
        /// The initial password confirm
        /// </summary>
        [Required, MinLength(10), Compare("Password")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// The user's Job Description
        /// </summary>
        [MaxLength(100)]
        public string JobDescription { get; set; }

        /// <summary>
        /// The user's role
        /// </summary>
        [Required]
        public UserRole Role { get; set; }

        /// <summary>
        /// The account state
        /// </summary>
        [Required]
        public AccountState State { get; set; }
    }
}
