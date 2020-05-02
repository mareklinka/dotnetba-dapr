using System.Threading;
using System;
using Dapr.Client;
using DotNetBa.Dapr.UserService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DotNetBa.Dapr.UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<LoginResponse> Login(LoginModel model, [FromServices] DaprClient dapr, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login request received - processing...");

            if (model.Username == "error")
            {
                throw new Exception("this is my error message");
            }

            await dapr.PublishEventAsync("notification_login", new { model.Username, Timestamp = DateTime.Now }, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Login request approved.");

            return new LoginResponse { IsSuccess = true, Roles = new[] { "role_1", "role_x" }, Username = model.Username };
        }
    }
}
