namespace MediBook.Core.DTOs
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DTO for user authentication
    /// </summary>
    public class UserForAuthenticationDto
    {
        /// <summary>
        /// The username
        /// </summary>
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }

        /// <summary>
        /// The user's password
        /// </summary>
        [Required(ErrorMessage = "Password name is required")]
        public string Password { get; set; }
    }
}
