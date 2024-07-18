using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationInfrastructure.Contracts;
using MediatR;

namespace Applications.CQRS.User.Command.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserContext _userContext;
        public RegisterUserCommandHandler(IUserContext userContext)
        {
            _userContext = userContext;
            
        }
        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            await _userContext.registerAsync(request.Request);
        }
    }
}