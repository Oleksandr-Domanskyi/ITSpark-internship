using ApplicationCore.Domain.Authorization;
using ApplicationCore.Dto.Request;
using Applications.CQRS.User.Command.Login;
using Applications.CQRS.User.Command.Register;
using Applications.CQRS.User.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IT_Sprark_Sprawdzenie_Wiedzy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMediator _mediator;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequest model)
        {
            await _mediator.Send(new RegisterUserCommand(model));
            return Ok("Register was successful");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRequest model)
        {
            if (await _mediator.Send(new LoginUserCommand(model)))
            {
                return Ok("Logined was successful");
            }
            else
            {
                return BadRequest("Login was Failed!!!");
            }
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var model = await _mediator.Send(new GetCurrentUserQuery());
            return Ok(model);
        }
    }
}
