using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Dto.Request;
using MediatR;

namespace Applications.CQRS.User.Command.Register
{
    public class RegisterUserCommand:IRequest
    {
        public UserRequest Request { get; set; }

        public RegisterUserCommand(UserRequest request)
        {
            Request = request;
        }
    }
}