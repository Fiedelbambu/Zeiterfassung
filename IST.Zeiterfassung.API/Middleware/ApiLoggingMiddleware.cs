using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IST.Zeiterfassung.API.Middleware
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiLoggingMiddleware> _logger;
        private static readonly string[] SensitiveKeys = new[] { "passwort", "password", "pwd", "secret", "token" };

        public ApiLoggingMiddleware(RequestDelegate next, ILogger<ApiLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            var requestBody = string.Empty;
            if (context.Request.ContentLength > 0 && context.Request.ContentType?.Contains("application/json") == true)
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                requestBody = MaskSensitiveData(requestBody);
            }

            _logger.LogInformation("HTTP Request: {method} {url} {body}",
                context.Request.Method,
                context.Request.Path,
                requestBody);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation("HTTP Response: {statusCode} {body}",
                context.Response.StatusCode,
                responseText);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private string MaskSensitiveData(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var masked = MaskJsonElement(doc.RootElement);
                return JsonSerializer.Serialize(masked);
            }
            catch
            {
                return json; // Fallback, falls keine gültige JSON
            }
        }

        private Dictionary<string, object?> MaskJsonElement(JsonElement element)
        {
            var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            foreach (var property in element.EnumerateObject())
            {
                if (SensitiveKeys.Contains(property.Name.ToLower()))
                {
                    dict[property.Name] = "***MASKIERT***";
                }
                else if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    dict[property.Name] = MaskJsonElement(property.Value);
                }
                else
                {
                    dict[property.Name] = property.Value.ToString();
                }
            }

            return dict;
        }
    }
}
