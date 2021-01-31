namespace MediBook.Web.Extensions
{
    using MediBook.Data.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

    }
}