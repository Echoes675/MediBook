namespace MediBook.Data.Repositories
{
    using System.Threading.Tasks;

    public class RepositoryManager : IRepositoryManager
    {
        public IJobDescriptionDal JobDescription { get; }
        public Task SaveAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}