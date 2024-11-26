using DevExtreme.AspNet.Data.ResponseModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomData.Extensions
{
    public static class LoadResultExtensions
    {
        public static readonly JsonSerializerOptions Options = GetOptions();

        public static JsonSerializerOptions GetOptions()
        {
            JsonSerializerOptions jsonSerializerOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            return jsonSerializerOptions;
        }

        public static IEnumerable<T> ToEnumerable<T>(this LoadResult loadResult)
        {
            if (loadResult?.data == null)
            {
                return Enumerable.Empty<T>();
            }

            // Check if the data contains JsonElement objects and deserialize them into the specified
            // type T.
            if (loadResult.data.OfType<JsonElement>().Any())
            {
                var jsonElements = loadResult.data.OfType<JsonElement>();

                return jsonElements.Select(e => e.Deserialize<T>(Options))!;
            }
            // If the data is already of type IEnumerable<T>, assign it directly.
            else
            {
                return loadResult.data is IEnumerable<T> elementObjects ? elementObjects : Enumerable.Empty<T>();
            }
        }
    }
}
