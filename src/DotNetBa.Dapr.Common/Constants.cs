namespace DotNetBa.Dapr.Common
{
    public static class Constants
    {
        public static class Topics
        {
            public const string LoginNotification = "notification_login";
        }

        public static class Storage
        {
            public const string RedisName = "statestore";
            public const string SqlName = "sql-server";
        }

        public static class Secrets
        {
            public const string AzureKeyVaultName = "azurekeyvault";
        }
    }
}
