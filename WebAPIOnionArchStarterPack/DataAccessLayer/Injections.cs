// Copyrights(c) Charqe.io. All rights reserved.

using DataAccessLayer.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
    public static class Injections
    {
        /// <summary>
        /// Adding data access layer's dependencies.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <returns></returns>
        public static IServiceCollection AddDataAccessLayerDependencies(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<ApplicationDbContext>();
            
            return services;
        }

        /// <summary>
        /// Configuring identity builder object.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IdentityBuilder ConfigureIdentityBuilder(this IdentityBuilder builder)
        {
            builder.AddEntityFrameworkStores<ApplicationDbContext>();

            return builder;
        }
            
    }
}