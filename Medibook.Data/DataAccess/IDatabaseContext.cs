namespace Medibook.Data.DataAccess
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediBook.Core.Models;
    using Microsoft.EntityFrameworkCore;

    public interface IDatabaseContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class, IDbEntity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}