using System;

namespace Common.Exceptions
{
	public sealed class BadRequestException : Exception
    {
		public BadRequestException(string message)
            : base(message)
        {
        }
    }
}