using System;
using DataAccessLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            InitializeDatabase(host);

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        /// <summary>
        /// Migrates database and executes other tasks related of database.
        /// </summary>
        /// <param name="host"></param>
        private static void InitializeDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            try
            {
                DbInitializer.Initialize(scope.ServiceProvider);
            }
            catch (Exception ex)
            {
                // TODO: we are going to log exception to logger db.
            }
        } 
    }
}