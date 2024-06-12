using ApplicationCore.Domain.Entity.ItemProfile;
using Applications.Dto;
using Applications.Dto.Request;
using Applications.Extentions.MediatRHandler;
using Applications.Mapper;
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
            services.AddAutoMapper(typeof(ApplicationMapperProfile));
            MediatrServices(services);
        }

        private static void MediatrServices(IServiceCollection services)
        {

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<Mediator>();
            });

            // Add All Entities
            MediatRHandler.MediatRHandler.MediatrRegisterHandler<ItemProfile, ItemProfileDto, ItemProfileRequest>(services);
        }


       
    }
}
