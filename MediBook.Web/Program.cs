namespace MediBook.Web
{
    using System;
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
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
                        logBuilder.SetMinimumLevel(LogLevel.Information);
                        logBuilder.AddLog4Net("log4net.config");
                    }).UseConsoleLifetime();
    }
}
