using System;
using DotNetBa.Dapr.UserService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public LoginResponse Login(LoginModel model)
        {
            _logger.LogInformation("Login request received - processing...");

            if (model.Username == "error")
            {
                throw new Exception("this is my error message");
            }

            _logger.LogInformation("Login request approved.");

            return new LoginResponse { IsSuccess = true, Roles = new[] { "role_1", "role_x" }, Username = model.Username };
        }
    }
}
