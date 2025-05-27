namespace MediBook.Web
{
    using System;
    using System.Threading.Tasks;
    using MediBook.Data.DataAccess;
    using MediBook.Web.Seeders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var dbContext = services.GetRequiredService<MediBookDatabaseContext>();
                    Console.WriteLine("Applying migrations...");
                    await dbContext.Database.MigrateAsync();

                    Console.WriteLine("Seeding data...");
                    await SeedUserData.Seed(services, dbContext);
                    await SeedPatientsData.Seed(services, dbContext);
                    Console.WriteLine("Data seeding completed.");
                }

                await host.RunAsync();
            }
            catch (Exception e)
            {
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(logBuilder =>
                {
                    logBuilder.ClearProviders();
                    logBuilder.SetMinimumLevel(LogLevel.Information);
                    logBuilder.AddLog4Net("log4net.config");
                }).UseConsoleLifetime();
    }
}
