namespace DotNetBa.Dapr.Common.Models
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }

        public string[] Roles { get; set; }

        public string Username { get; set; }
    }
}
