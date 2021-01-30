namespace Medibook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using Medibook.Data.DataAccess;
    using Microsoft.EntityFrameworkCore;

    public abstract class RepositoryBase<TEntity>
           where TEntity : class, IDbEntity
    {
        protected readonly IDatabaseContext Db;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity}"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="databaseContext">
        /// The database Context.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// DatabaseContext not provided.
        /// </exception>
        protected RepositoryBase(IDatabaseContext databaseContext)
        {
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
                return null;
            }

            entity = await DbCreateAsync(entity).ConfigureAwait(false);
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
                return entity;
            }

            return null;
        }

        /// <summary>
        /// Returns all the entities in a particular DbSet
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var retVal = await Db.Set<TEntity>().AsNoTracking().ToListAsync().ConfigureAwait(false);
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
            var entities = await GetAllAsync().ConfigureAwait(false);
            return entities.FirstOrDefault(s => s.Id == id) != null;
        }

        /// <summary>
        /// Filter entities in a DbSet based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.InvalidOperationException">Condition.</exception>
        public IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (Db.Set<TEntity>().AsNoTracking().AsEnumerable() ?? throw new InvalidOperationException(nameof(TEntity))).Where(predicate);
        }

        /// <summary>
        /// Returns all entities as IQueryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> DbGetAll()
        {
            return Db.Set<TEntity>().AsNoTracking();
        }

        protected Task<TEntity> DbGetByIdAsync(int id)
        {
            return Db.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Updates an entity in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity"/> is <see langword="null"/></exception>
        protected async Task<TEntity> DbUpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Db.Set<TEntity>().Update(entity);
            await Db.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

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

        protected async Task<TEntity> DbCreateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Db.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
            return entity;

        }
    }
}