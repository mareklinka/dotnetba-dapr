using Dapr;
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
        public IActionResult Post([FromBody] LoginNotificationRequest model)
        {
            _logger.LogInformation($"Attempting to deliver a login notification for user {model.Username}");

            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                _logger.LogWarning($"Unable to send notification for user {model.Username}");
                return Ok();
            }

            _logger.LogInformation($"Sending notification to {model.Phone}");

            return Ok();
        }
    }
}
