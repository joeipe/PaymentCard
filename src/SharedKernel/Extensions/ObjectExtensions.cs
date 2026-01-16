using System.Text.Json;

namespace SharedKernel.Extensions
{
    public static class ObjectExtensions
    {
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