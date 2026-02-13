using System.Text;
using System.Text.Json;

namespace ProductApp.Web.Exceptions;

internal static class HttpRequestExceptionExtension
{
    public static string BuildErrorMessage(this HttpRequestException httpException)
    {
        const string defaultMessage = "Request failed.";

        if (string.IsNullOrWhiteSpace(httpException.Message))
        {
            return defaultMessage;
        }

        try
        {
            var document = JsonDocument.Parse(httpException.Message);
            var root = document.RootElement;

            var builder = new StringBuilder();
            if (root.TryGetProperty("message", out var message))
            {
                builder.Append(message.GetString());
            }

            if (!root.TryGetProperty("errors", out var errors) || errors.ValueKind != JsonValueKind.Object) return builder.Length > 0 ? builder.ToString() : httpException.Message;

            foreach (var errorMessage in errors.EnumerateObject().SelectMany(property => property.Value.EnumerateArray()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(' ');
                }

                builder.Append(errorMessage.GetString());
            }

            return builder.Length > 0 ? builder.ToString() : httpException.Message;
        }
        catch (JsonException)
        {
            return httpException.Message;
        }
    }
}