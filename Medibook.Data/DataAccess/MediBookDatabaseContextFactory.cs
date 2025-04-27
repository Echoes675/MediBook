namespace MediBook.Data.DataAccess;

using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class MediBookDatabaseContextFactory : IDesignTimeDbContextFactory<MediBookDatabaseContext>
{
    public MediBookDatabaseContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Medibook.Web"))
            .AddJsonFile("appsettings.json")
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<MediBookDatabaseContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSQLConnection"));
        return new MediBookDatabaseContext(optionsBuilder.Options);
    }
}
