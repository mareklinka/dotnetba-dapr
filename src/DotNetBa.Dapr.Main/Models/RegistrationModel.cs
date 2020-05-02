namespace DotNetBa.Dapr.Main.Models
{
    public class RegistrationModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public bool Validate() => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Phone);
    }
}