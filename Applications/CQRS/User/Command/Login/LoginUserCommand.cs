using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Dto.Request;
using MediatR;

namespace Applications.CQRS.User.Command.Login
{
    public class LoginUserCommand:IRequest<bool>
    {
        public UserRequest Request { get; set; }
        public LoginUserCommand(UserRequest request)
        {
            Request = request;
        }
        
    }
}