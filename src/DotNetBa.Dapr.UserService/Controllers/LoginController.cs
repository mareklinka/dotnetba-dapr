using System.Threading;
using System;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DotNetBa.Dapr.Common.Models;

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
        public async Task<LoginResponse> Login(LoginRequest model, [FromServices] DaprClient dapr, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login request received - processing...");

            if (model.Username == "error")
            {
                throw new Exception("this is my error message");
            }

            var request = new LoginNotificationRequest { Username = model.Username, Timestamp = DateTime.Now };
            await dapr.PublishEventAsync("notification_login", request, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Login request approved.");

            return new LoginResponse { IsSuccess = true, Roles = new[] { "role_1", "role_x" }, Username = model.Username };
        }
    }
}
