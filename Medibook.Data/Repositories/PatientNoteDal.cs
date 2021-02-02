namespace MediBook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    public interface IPatientNoteDal
    {
        /// <summary>
        /// Method to Add an Entity to the database after confirming it doesn't already exist
        /// </summary>
        /// <param name="entity">The entity parameter</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<PatientNote> AddAsync(PatientNote entity);

        /// <summary>
        /// Method to delete an entity from the database if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Method to Update an Entity to the database after confirming it doesn't already exist
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<PatientNote> UpdateAsync(PatientNote entity);

        /// <summary>
        /// Returns all the entities in a particular DbSet
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PatientNote>> GetAllAsync();

        /// <summary>
        /// Returns an entity of a given type using its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PatientNote> GetEntityAsync(int id);

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        Task<bool> CheckEntityExistsAsync(PatientNote entity);

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> CheckEntityExistsAsync(int id);

        /// <summary>
        /// Filter entities in a DbSet based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.InvalidOperationException">Condition.</exception>
        IEnumerable<PatientNote> Filter(Func<PatientNote, bool> predicate);
    }

    /// <summary>
    /// PatientNoteDal
    /// </summary>
    public class PatientNoteDal : RepositoryBase<PatientNote>, IPatientNoteDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientNoteDal"/> class
        /// </summary>
        /// <param name="databaseContext"></param>
        public PatientNoteDal(IDatabaseContext databaseContext, ILogger<PatientNoteDal> logger) : base(databaseContext, logger)
        {
        }
    }
}