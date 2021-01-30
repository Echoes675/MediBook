namespace MediBook.Core.Models
{
    public interface IDbEntity
    {
        /// <summary>
        /// The Id of the entity
        /// </summary>
        int Id { get; set; }
    }
}