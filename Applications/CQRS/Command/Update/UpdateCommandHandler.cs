using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Services;
using Applications.Services.UserService;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Command.Update
{
    public class UpdateCommandHandler<TDomain, TReq> : IRequestHandler<UpdateCommand<TDomain, TReq>>
        where TDomain : Entity<Guid>
        where TReq : class
    {
        private readonly IEntityService<TDomain, TReq> _service;
        private readonly ICheckUserService<TDomain, TReq> _checkUserService;

        public UpdateCommandHandler(IEntityService<TDomain, TReq> service,ICheckUserService<TDomain,TReq> checkUserService)
        {
            _service = service;
            _checkUserService = checkUserService;
        }
        public async Task Handle(UpdateCommand<TDomain, TReq> request, CancellationToken cancellationToken)
        {
            if(await _checkUserService.CheckUserAsync((Guid)typeof(TDomain).GetProperty("Id")?.GetValue(request.request)!))
            {
                await _service.UpdateAsync(request.request, request.Id);
            }
            else
            {
                throw new Exception("Access denied");
            }
            
        }


    }
}
