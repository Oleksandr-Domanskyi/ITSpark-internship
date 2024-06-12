using ApplicationInfrastructure.Data;
using ApplicationInfrastructure.Data.Seed;
using ApplicationInfrastructure.Repositories;
using ApplicationInfrastructure.Repositories.UnitOfWork;
using ApplicationInfrastructure.Services;
using Applications.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInfrastructure.Extention
{
    public static class ApplicationInfrastructureExtentions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));

            services.AddScoped<ApplicationSeeder>();

            // Add unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Add api services
            services.AddScoped(typeof(IEntityService<>), typeof(EntityServices<>));


        }
    }
}
