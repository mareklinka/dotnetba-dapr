using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Client;
using DotNetBa.Dapr.Common;
using DotNetBa.Dapr.Common.Models;
using DotNetBa.Dapr.Main.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static DotNetBa.Dapr.Common.Constants;

namespace DotNetBa.Dapr.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger) => _logger = logger;

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginModel model,
                                              [FromServices] DaprClient dapr,
                                              CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User {model.Username} is attempting to log in");

            if (!model.Validate())
            {
                throw new AuthenticationException("Invalid login model");
            }

            var secret = await dapr.GetSecretAsync(Secrets.AzureKeyVaultName, "test-secret", cancellationToken: cancellationToken)
                                   .ConfigureAwait(false);

            await dapr.SaveStateAsync(Storage.SqlName, "last-login-attempt", model, cancellationToken: cancellationToken)
                      .ConfigureAwait(false);

            var request = new LoginRequest { Username = model.Username, Password = model.Password };
            var response =
                await dapr.InvokeMethodAsync<LoginRequest, LoginResponse>(Apps.UserService,
                                                                          "login/login",
                                                                          request,
                                                                          cancellationToken: cancellationToken)
                          .ConfigureAwait(false);

            if (!response.IsSuccess)
            {
                return Forbid();
            }

            var proxy = ActorProxy.Create<IFancyActor>(new ActorId("1"), "FancyActor");
            var actorResponse = await proxy
                .PerformComplexCalculation(new FancyActorState() { Data = "My test data" })
                .ConfigureAwait(false);

            _logger.LogInformation($"Actor produced value \"{actorResponse}\" on behalf of the user");
            _logger.LogInformation($"User {model.Username} is now logged in");

            return Ok();
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegistrationModel model,
                                                 [FromServices] DaprClient dapr,
                                                 CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User {model.Username} is attempting to register");

            if (!model.Validate())
            {
                throw new AuthenticationException("Invalid login model");
            }

            await dapr.InvokeMethodAsync(Apps.UserService,
                                         "login/register",
                                         new RegistrationRequest { Username = model.Username, Phone = model.Phone },
                                         cancellationToken: cancellationToken)
                      .ConfigureAwait(false);

            _logger.LogInformation($"User {model.Username} is now registered");

            return Ok();
        }
    }
}
