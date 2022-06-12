// Copyrights(c) Charqe.io. All rights reserved.

using BusinessLogicLayer.Authorization;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace BusinessLogicLayer
{
    public static class Injections
    {
        /// <summary>
        /// Adding business logic layer's dependencies.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessLayerDependencies(this IServiceCollection services)
        {
            services.AddTransient<IAppUserService<AppUser>, AppUserService>();

            return services;
        }

        /// <summary>
        /// Adding authorization handler dependencies.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(BllConstants.Authorization.Basic, policy => policy.Requirements.Add(new BasicAuthorizationRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, BasicAuthorizationHandler>();

            return services;
        }
    }
}