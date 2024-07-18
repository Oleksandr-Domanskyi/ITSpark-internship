using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Ceneo;
using Applications.CQRS.Command.Create;
using Applications.CQRS.Command.Delete;
using Applications.CQRS.Command.Update;
using Applications.CQRS.Queries.GetAll;
using Applications.CQRS.Queries.GetById;
using Applications.CQRS.Queries.GetProductPriceFromCeneo;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Extentions.MediatRHandler
{
    public class MediatRHandler
    {
        public static void MediatrRegisterHandler<TDomain, TDto, TReq>(IServiceCollection services)
           where TDomain : Entity<Guid>
           where TDto : class
           where TReq : class
        {
            CeneoMediatrRegister(services);

            services.AddTransient(
                typeof(IRequestHandler<GetAllQuery<TDomain, TDto>, IEnumerable<TDto>>),
                typeof(GetAllQueryHandler<TDomain, TDto>)
            );
            services.AddTransient(
               typeof(IRequestHandler<GetByIdQuery<TDomain, TDto>, TDto>),
               typeof(GetByIdQueryHandler<TDomain, TDto>)
           );

            services.AddTransient(
                typeof(IRequestHandler<CreateCommand<TDomain, TReq>>),
                typeof(CreateCommandHandler<TDomain, TReq>)
            );
            services.AddTransient(
                typeof(IRequestHandler<UpdateCommand<TDomain, TDto, TReq>>),
                typeof(UpdateCommandHandler<TDomain, TDto, TReq>)
            );
            services.AddTransient(
                typeof(IRequestHandler<DeleteCommand<TDomain, TDto>>),
                typeof(DeleteCommandHandler<TDomain, TDto>)
            );
        }
        private static void CeneoMediatrRegister(IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetPriceByCeneoQuery, CeneoProduct>, GetPriceByCeneoQueryHandler>();
        }
    }
}
