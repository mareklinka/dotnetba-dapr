using System;

namespace DotNetBa.Dapr.Common.Models
{
    public class LoginNotificationRequest
    {
        public string Username { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
