using System.Threading;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using DotNetBa.Dapr.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static DotNetBa.Dapr.Common.Constants;

namespace DotNetBa.Dapr.NotificationService.Controllers
{
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger) => _logger = logger;

        [Topic(Topics.LoginNotification)]
        [HttpPost(Topics.LoginNotification)]
        public async Task<IActionResult> Post(LoginNotificationRequest model, [FromServices] DaprClient dapr, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Attempting to deliver a login notification for user {model.Username}");

            var profile = await dapr
                .GetStateEntryAsync<UserProfile>(Storage.Name, model.Username, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (profile.Value is null)
            {
                _logger.LogWarning($"Unable to load user profile for user {model.Username}");
                return Ok();
            }

            _logger.LogInformation($"User profile for {model.Username} loaded.");
            _logger.LogInformation($"Sending notification to {profile.Value.PhoneNumber}");

            return Ok();
        }
    }
}
