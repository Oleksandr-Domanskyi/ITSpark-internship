using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity.Product;
using ApplicationCore.Dto;
using ApplicationCore.Dto.Response;
using ApplicationInfrastructure.Contracts;
using Applications.CQRS.User.Command.Register;
using Applications.CQRS.User.Queries.GetCurrentUser;
using Applications.Dto;
using Applications.Dto.Request;
using Applications.Services.CeneoService;
using Applications.Services.UserService;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Extentions
{
    public static class ApplicationExtentions
    {
        public static void AddApplication(this IServiceCollection services)
        {

            MediatrServices(services);

            services.AddTransient<ISearchProductCeneoService, SearchProductCeneoService>();


            services.AddTransient(typeof(ICheckUserService<,>), typeof(CheckUserService<,>));
        }

        private static void MediatrServices(IServiceCollection services)
        {

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<Mediator>();
                cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommand>();
            });


            // Add All Entities
            MediatRHandler.MediatRHandler.MediatrRegisterHandler<Product, ProductDto, ProductRequest>(services);
        }



    }
}
