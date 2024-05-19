using System;

namespace Common.Exceptions
{
	public sealed class ConflictException : Exception
    {
		public ConflictException(string message, Exception? innerMessage)
            : base(message, innerMessage)
        {
        }
    }
}