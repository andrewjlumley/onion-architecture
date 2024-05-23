using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Exceptions
{
	public class GlobalExceptionHandler : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception e)
			{
				await HandleExceptionAsync(context, e);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
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
			await httpContext.Response.WriteAsJsonAsync(error);
		}
	}
}
