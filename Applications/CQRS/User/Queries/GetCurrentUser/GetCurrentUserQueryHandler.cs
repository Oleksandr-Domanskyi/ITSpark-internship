using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Dto.Response;
using ApplicationInfrastructure.Contracts;
using MediatR;

namespace Applications.CQRS.User.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserResponse>
    {
        private readonly IUserContext _userContext;
        public GetCurrentUserQueryHandler(IUserContext userContext )
        {
            _userContext = userContext;
        }
        public async Task<UserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
             return await _userContext.GetCurrentUser();
        }
    }
}