namespace MediBook.Core.Models
{
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
        public string Description { get; set; }
    }
}