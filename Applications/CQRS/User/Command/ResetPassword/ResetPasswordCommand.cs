using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Dto.Request;
using MediatR;

namespace Applications.CQRS.User.Command.ResetPassword
{
    public class ResetPasswordCommand : IRequest
    {
        public ResetPasswordRequest ResetPassword { get; set; }

        public ResetPasswordCommand(ResetPasswordRequest resetPassword)
        {
            ResetPassword = resetPassword;
        }

    }
}