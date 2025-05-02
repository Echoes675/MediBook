namespace MediBook.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using MediBook.Core.Enums;

    /// <summary>
    /// The user's JobDescription
    /// </summary>
    public class JobDescription : IDbEntity
    {
        /// <summary>
        /// The JobDescription Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The JobDescription
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the role associated with the job description.
        /// </summary>
        /// <remarks>
        /// The <see cref="UserRole"/> defines the specific role or responsibility 
        /// assigned to the user, such as Medical Practitioner, Reception, or Practice Admin.
        /// </remarks>
        public UserRole Role { get; set; }
    }
}