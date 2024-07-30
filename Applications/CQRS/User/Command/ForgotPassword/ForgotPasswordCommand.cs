using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Applications.CQRS.User.Command.ForgotPassword
{
    public class ForgotPasswordCommand:IRequest
    {
        public string Email { get; set;}

        public ForgotPasswordCommand(string email)
        {
            Email = email;
        }
    }
}