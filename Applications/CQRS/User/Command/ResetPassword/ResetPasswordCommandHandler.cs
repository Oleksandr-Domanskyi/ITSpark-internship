using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationInfrastructure.Contracts;
using MediatR;

namespace Applications.CQRS.User.Command.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUserContext _userContext;

        public ResetPasswordCommandHandler(IUserContext userContext)
        {
            _userContext = userContext;
        }
        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _userContext.ResetPasswordAsync(request.ResetPassword);
        }
    }
}