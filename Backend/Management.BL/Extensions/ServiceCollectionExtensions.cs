using Management.DL.Context;
using Management.DL.Models;
using Management.DL.Repositories.Interface;
using Management.DL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Management.BL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IRepository<Product>, Repository<Product>>();
            services.AddScoped<IRepository<Category>, Repository<Category>>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Management.DL") // Specify the migrations assembly
                )
            );
            return services;
        }
    }
}