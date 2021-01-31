namespace MediBook.Data.Repositories
{
    using System.Threading.Tasks;

    public interface IRepositoryManager
    {
        IJobDescriptionDal JobDescription { get; }
        Task SaveAsync();
    }
}