using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationInfrastructure.Contracts;
using MediatR;

namespace Applications.CQRS.User.Command.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, bool>
    {
        private readonly IUserContext _userContext;

        public LoginUserCommandHandler(IUserContext userContext)
        {
            _userContext = userContext;
        }
        public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await _userContext.loginAsync(request.Request);
        }
    }
}