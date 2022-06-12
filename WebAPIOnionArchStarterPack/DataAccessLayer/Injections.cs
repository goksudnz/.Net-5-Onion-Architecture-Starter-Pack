// Copyrights(c) Charqe.io. All rights reserved.

using DataAccessLayer.Generic;
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
            => services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}