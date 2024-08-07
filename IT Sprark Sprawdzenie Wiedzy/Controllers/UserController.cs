using ApplicationCore.Dto.Request;
using Applications.CQRS.User.Command.ForgotPassword;
using Applications.CQRS.User.Command.Login;
using Applications.CQRS.User.Command.Register;
using Applications.CQRS.User.Command.ResetPassword;
using Applications.CQRS.User.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IT_Sprark_Sprawdzenie_Wiedzy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
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
            return await _mediator.Send(new LoginUserCommand(model))
                ? Ok("Logined was successful")
                : BadRequest("Login was Failed!!!");

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> CurrentUser()
        {
            var model = await _mediator.Send(new GetCurrentUserQuery());
            return Ok(model);
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string Email)
        {
            await _mediator.Send(new ForgotPasswordCommand(Email));
            return Ok();
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordRequest request)
        {
            await _mediator.Send(new ResetPasswordCommand(request));
            return Ok();
        }
    }
}
