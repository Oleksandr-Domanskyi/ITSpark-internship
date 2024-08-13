using ApplicationCore.Domain.Azure;
using ApplicationCore.Domain.Smtp;
using ApplicationInfrastructure.Contracts;
using ApplicationInfrastructure.Data;
using ApplicationInfrastructure.Data.Seed;
using ApplicationInfrastructure.Repositories.UnitOfWork;
using ApplicationInfrastructure.Repositories.UserContext;
using ApplicationInfrastructure.Services;
using ApplicationInfrastructure.Services.EmailSender;
using ApplicationInfrastructure.Services.ImageService;
using ApplicationInfrastructure.Services.ImageService.GetOldImagePathService;
using Applications.Contracts;
using Applications.Events.DeleteImageFromAzure;
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

            // Configuration reference with appsettings
            services.Configure<AzureOptions>(configuration.GetSection("Azure"));
            services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));

            // Add other services
            services.AddScoped<ApplicationSeeder>();

            // Add unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Mapper
            services.AddAutoMapper(typeof(ApplicationMapperProfile));

            // Add API services
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IPDFProductGeneratorService, PDFProductGeneratorService>();

            services.AddScoped(typeof(IEntityService<,>), typeof(EntityServices<,>));
            services.AddScoped(typeof(ISpecification<>), typeof(Specification<>));
            services.AddScoped(typeof(IImageAzureService<,>), typeof(ImageAzureService<,>));
            services.AddScoped(typeof(IImageManagerService<,>), typeof(ImageManagerService<,>));
            services.AddScoped(typeof(IOldImagePathService<>), typeof(OldImagePathService<>));
            services.AddScoped(typeof(IDeleteImageFromAzureEvent<,>), typeof(DeleteImageFromAzureEvent<,>));


        }
    }
}
