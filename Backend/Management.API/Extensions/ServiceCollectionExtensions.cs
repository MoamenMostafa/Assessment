using Management.BL.MappingProfiles;
using Management.BL.Services;
using Management.BL.Services.Interface;
using Management.BL.Services.Management.BL.Services;
using Management.DL.Models;
using Management.DL.Repositories.Interface;
using Management.DL.Repositories;
using Microsoft.EntityFrameworkCore;
using Management.DL.Context;

namespace Management.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}