using System.Text.Json;

namespace SharedKernel.Extensions
{
    public static class ObjectExtensions
    {
        public static string OutputJson(this object item)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(item, serializeOptions);
            return json;
        }

        public static T OutputObject<T>(this string json)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<T>(json, serializeOptions);
            return result;
        }
    }
}