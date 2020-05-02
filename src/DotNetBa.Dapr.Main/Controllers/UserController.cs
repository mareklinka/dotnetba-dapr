using System.Security.Authentication;
using System.Threading;
using System.Text.Json;
using System.Threading.Tasks;
using Dapr.Client;
using DotNetBa.Dapr.Main.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetBa.Dapr.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginModel model, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User {model.Username} is attempting to log in");

            if (!model.Validate())
            {
                throw new AuthenticationException("Invalid login model");
            }

            var dapr = new DaprClientBuilder().UseJsonSerializationOptions(new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }).Build();

            var response =
                await dapr.InvokeMethodAsync<LoginModel, LoginResponse>("userservice",
                                                                        "login/login",
                                                                        model,
                                                                        cancellationToken: cancellationToken)
                        .ConfigureAwait(false);

            if (!response.IsSuccess)
            {
                return Forbid();
            }

            _logger.LogInformation($"User {model.Username} is now logged in");

            return Ok();
        }
    }
}
