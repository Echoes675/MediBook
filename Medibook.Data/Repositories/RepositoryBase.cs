namespace MediBook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using MediBook.Data.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The Repository base class
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class RepositoryBase<TEntity> where TEntity : class, IDbEntity
    {
        /// <summary>
        /// The Db context
        /// </summary>
        protected readonly IDatabaseContext Db;

        /// <summary>
        /// The logger
        /// </summary>
        private protected readonly ILogger _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity}"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="databaseContext">
        /// The database Context.
        /// </param>
        /// <param name="logger"></param>
        /// <exception cref="T:System.ArgumentNullException">
        /// MediBookDatabaseContext not provided.
        /// </exception>
        protected RepositoryBase(IDatabaseContext databaseContext, ILogger<RepositoryBase<TEntity>> logger)
        {
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            Db = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        }

        /// <summary>
        /// Method to Add an Entity to the database after confirming it doesn't already exist
        /// </summary>
        /// <param name="entity">The entity parameter</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                
                throw new ArgumentNullException(nameof(entity));
            }

            if (await CheckEntityExistsAsync(entity).ConfigureAwait(false))
            {
                _log.LogWarning($"Entity cannot be added as it already exists. \"Id\"={entity.Id} \"EntityType\"={typeof(TEntity)}");
                return null;
            }

            entity = await DbCreateAsync(entity).ConfigureAwait(false);
            _log.LogDebug($"Entity added to the db. \"Id\"={entity.Id} \"EntityType\"={typeof(TEntity)}");

            return entity;
        }

        /// <summary>
        /// Method to delete an entity from the database if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var result = await DbDeleteAsync(id).ConfigureAwait(false);
            _log.LogDebug($"Entity deleted from the db. \"Id\"={id} \"EntityType\"={typeof(TEntity)}");

            return result;
        }

        /// <summary>
        /// Method to Update an Entity to the database after confirming it doesn't already exist
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (await CheckEntityExistsAsync(entity.Id).ConfigureAwait(false))
            {
                await DbUpdateAsync(entity).ConfigureAwait(false);
                _log.LogDebug($"Entity updated in the db. \"Id\"={entity.Id} \"EntityType\"={typeof(TEntity)}");
                return entity;
            }

            _log.LogWarning($"Entity cannot be updated as it does not exist in the db. \"Id\"={entity.Id} \"EntityType\"={typeof(TEntity)}");
            return null;
        }

        /// <summary>
        /// Returns all the entities in a particular DbSet
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var retVal = await Db.Set<TEntity>().ToListAsync().ConfigureAwait(false);
            _log.LogDebug($"All Entities returned of type. \"EntityType\"={typeof(TEntity)}");
            return retVal;
        }

        /// <summary>
        /// Returns an entity of a given type using its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetEntityAsync(int id)
        {
            var entities = await GetAllAsync().ConfigureAwait(false);

            return entities.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        public virtual Task<bool> CheckEntityExistsAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return CheckEntityExistsAsync(entity.Id);
        }

        /// <summary>
        /// Check if entity exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> CheckEntityExistsAsync(int id)
        {
            return await Db.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id) != null;
        }

        /// <summary>
        /// Filter entities in a DbSet based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.InvalidOperationException">Condition.</exception>
        public virtual IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (Db.Set<TEntity>().AsEnumerable() ?? throw new InvalidOperationException(nameof(TEntity))).Where(predicate);
        }

        /// <summary>
        /// Retrieves the entity matching the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected Task<TEntity> DbGetByIdAsync(int id)
        {
            return Db.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Updates an entity in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        protected async Task<TEntity> DbUpdateAsync(TEntity entity)
        {
            Db.Set<TEntity>().Update(entity);
            await Db.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// Deletes the entity from the database with the given Id number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<bool> DbDeleteAsync(int id)
        {
            var entity = await DbGetByIdAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                return false;
            }

            Db.Set<TEntity>().Remove(entity);
            await Db.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// Adds the new entity to the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task<TEntity> DbCreateAsync(TEntity entity)
        {
            await Db.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }
    }
}