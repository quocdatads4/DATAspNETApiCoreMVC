using Microsoft.OpenApi.Models;

namespace DATAspNETApiCoreMVC.Middleware
{
	public class ApiKeyMiddleware
	{
		private readonly RequestDelegate _next;
		private const string APIKEY_NAME = "Apikey";

		public ApiKeyMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (!context.Request.Headers.TryGetValue(APIKEY_NAME, out var extractedApiKey))
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync("API Key was not provided.");
				return;
			}

			var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();

			var apiKey = appSettings.GetValue<string>(APIKEY_NAME);

			if (!apiKey.Equals(extractedApiKey))
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync("Unauthorized client.");
				return;
			}

			await _next(context);
		}
		
	}
}
