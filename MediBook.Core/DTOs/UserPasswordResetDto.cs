namespace MediBook.Core.DTOs
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Details required to reset a user password
    /// </summary>
    public class UserPasswordResetDto
    {
        /// <summary>
        /// The User's Id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The new password
        /// </summary>
        [Required, MinLength(10)]
        public string Password { get; set; }

        /// <summary>
        /// Confirmation of the new password
        /// </summary>
        [Required, MinLength(10), Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}