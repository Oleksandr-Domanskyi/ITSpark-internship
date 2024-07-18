using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Dto.Response;
using MediatR;

namespace Applications.CQRS.User.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<UserResponse>
    {

    }
}