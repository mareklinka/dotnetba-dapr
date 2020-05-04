using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using DotNetBa.Dapr.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static DotNetBa.Dapr.Common.Constants;

namespace DotNetBa.Dapr.UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger) => _logger = logger;

        [HttpPost]
        [Route("login")]
        public async Task<LoginResponse> Login([FromBody] LoginRequest model,
                                               [FromServices] DaprClient dapr,
                                               CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login request received - processing...");

            var profile = await dapr.GetStateEntryAsync<UserProfile>(Storage.RedisName,
                                                                     model.Username,
                                                                     cancellationToken: cancellationToken)
                                    .ConfigureAwait(false);

            if (profile.Value is null)
            {
                return new LoginResponse { IsSuccess = false };
            }

            var request = new LoginNotificationRequest
            {
                Username = model.Username,
                Timestamp = DateTime.Now,
                Phone = profile.Value.PhoneNumber
            };

            await dapr.PublishEventAsync(Topics.LoginNotification, request, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Login request approved.");

            return new LoginResponse
            {
                IsSuccess = true,
                Roles = new[] { "role_1", "role_x" },
                Username = model.Username
            };
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model,
                                                  [FromServices] DaprClient dapr,
                                                  CancellationToken cancellationToken)
        {
            _logger.LogInformation("Register request received - processing...");

            if (model.Username == "error")
            {
                throw new Exception("this is my error message");
            }

            var profile = new UserProfile { Name = model.Username, PhoneNumber = model.Phone };

            await dapr.SaveStateAsync(Storage.RedisName, profile.Name, profile, cancellationToken: cancellationToken)
                      .ConfigureAwait(false);

            _logger.LogInformation("Registration successful.");

            return Ok();
        }
    }
}
