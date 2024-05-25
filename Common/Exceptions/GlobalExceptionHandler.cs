using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Common.Exceptions
{
	public class GlobalExceptionHandler : IMiddleware
	{
		private ILogger<GlobalExceptionHandler> _logger;

		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
		{
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception e)
			{
				await HandleExceptionAsync(context, e, _logger);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception, ILogger<GlobalExceptionHandler> logger)
		{
			httpContext.Response.ContentType = "application/json";

			httpContext.Response.StatusCode = exception switch
			{
				BadRequestException => StatusCodes.Status400BadRequest,
				NotFoundException => StatusCodes.Status404NotFound,
				ConflictException => StatusCodes.Status409Conflict,
				InvalidException => StatusCodes.Status422UnprocessableEntity,
				_ => StatusCodes.Status500InternalServerError
			};

			var error = new { message = exception.Message, innerException = exception.InnerException, stackTrace = exception.StackTrace };
			logger.LogError(JsonSerializer.Serialize(new { applicationId = AppDomain.CurrentDomain.FriendlyName, error }));
			await httpContext.Response.WriteAsJsonAsync(new { message = $"{error.message} {error.innerException}".TrimEnd() });
		}
	}
}
