using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationInfrastructure.Contracts;
using MediatR;

namespace Applications.CQRS.User.Command.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserContext _userContext;

        public ForgotPasswordCommandHandler(IUserContext userContext)
        {
            _userContext = userContext;
        }
        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            await _userContext.ForgotPasswordAsync(request.Email);
        }
    }
}