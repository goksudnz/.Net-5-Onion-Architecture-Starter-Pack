// Copyrights(c) Charqe.io. All rights reserved.

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using DataAccessLayer.EntityConfigurations;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccessLayer
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>, IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// Constructor with database context options.
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        /// <summary>
        /// Creates database context.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = "db_connection_string";
            builder.UseNpgsql(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
        
        // we are adding database sets here.
        public virtual DbSet<AppUser> AppUsers { get; set; }

        /// <summary>
        /// Model creating process.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // we are adding entity configurations here.
            builder.ApplyConfiguration(new AppUserEntityConfigurations());
            
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Configuring process.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "db_connection_string";
            optionsBuilder.UseNpgsql(connectionString, option =>
            {
                option.RemoteCertificateValidationCallback(ValidateServerCertificate);
            });

            optionsBuilder.UseLazyLoadingProxies();
        }

        /// <summary>
        /// To ignore ssl certificate policy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

       
    }
}