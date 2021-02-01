namespace MediBook.Web.Models
{
    using MediBook.Core.Enums;

    /// <summary>
    /// Model to represent the User's details in the view
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// The entity's Id in the Users table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's role
        /// </summary>
        public UserRole Role { get; set; }

        /// <summary>
        /// The Account State
        /// </summary>
        public AccountState State { get; set; }
    }
}