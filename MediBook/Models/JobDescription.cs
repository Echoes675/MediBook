namespace MediBook.Core.Models
{
    using System.ComponentModel.DataAnnotations;

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
    }
}