namespace Medibook.Data.DataAccess
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="DatabaseContext"/> class
        /// </summary>
        /// <param name="options"></param>
        public DatabaseContext(DbContextOptions<DatabaseContext> options) 
            : base(options)
        {

        }

        /// <summary>
        ///     Creates a <see cref="DbSet{TEntity}" /> that can be used to query and save instances of <typeparamref name="TEntity" />.
        /// </summary>
        /// <typeparam name="TEntity"> The type of entity for which a set should be returned. </typeparam>
        /// <returns> A set for the given entity type. </returns>
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IDbEntity => base.Set<TEntity>();

        /// <summary>
        /// The save changes async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation Token.
        /// </param>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        /// An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        /// A concurrency violation is encountered while saving to the database.
        ///                 A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///                 This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);

        // Configure the context to use database. 
        /// <exception cref="T:System.ArgumentNullException"><paramref name="optionsBuilder"/> is <see langword="null"/></exception>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}