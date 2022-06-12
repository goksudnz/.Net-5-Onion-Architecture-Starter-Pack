// Copyrights(c) Charqe.io. All rights reserved.

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.ModelServices;

namespace PresentationLayer
{
    public static class Injections
    {
        /// <summary>
        /// Adding presentation layer's dependencies.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPresentationLayerDependencies(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(typeof(MappingProfile)));
            var mapper = mapperConfig.CreateMapper();

            services.AddTransient<UserModelService>();
            services.AddSingleton(mapper);

            return services;
        }
    }
}