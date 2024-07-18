using ApplicationCore.Domain.Azure;
using ApplicationInfrastructure.Contracts;
using ApplicationInfrastructure.Data;
using ApplicationInfrastructure.Data.Seed;
using ApplicationInfrastructure.Repositories;
using ApplicationInfrastructure.Repositories.UnitOfWork;
using ApplicationInfrastructure.Services;
using ApplicationInfrastructure.Services.ImageService;
using ApplicationInfrastructure.Services.ImageService.GetOldImagePathService;
using Applications.Contracts;
using Applications.Mapper;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationInfrastructure.Extention
{
    public static class ApplicationInfrastructureExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure the DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));

            // Configure AzureOptions
            services.Configure<AzureOptions>(configuration.GetSection("Azure"));

            // Add other services
            services.AddScoped<ApplicationSeeder>();

            // Add unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Add Mapper
            services.AddAutoMapper(typeof(ApplicationMapperProfile));

            // Add API services
            services.AddScoped(typeof(IEntityService<,>), typeof(EntityServices<,>));
            services.AddScoped(typeof(ISpecification<>), typeof(Specification<>));
            services.AddScoped(typeof(IImageAzureService<,>), typeof(ImageAzureService<,>));
            services.AddScoped(typeof(IOldImagePathService<>), typeof(OldImagePathService<>));
        }
    }
}
