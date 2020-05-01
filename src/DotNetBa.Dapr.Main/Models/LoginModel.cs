namespace DotNetBa.Dapr.Main.Models
{
    public class LoginModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool Validate() => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }
}