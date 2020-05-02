using System.Text.Json;

namespace DotNetBa.Dapr.Common
{
    public static class Serialization
    {
        public static readonly JsonSerializerOptions JSON = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}