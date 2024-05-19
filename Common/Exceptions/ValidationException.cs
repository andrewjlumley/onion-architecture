using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
	public sealed class InvalidException : Exception
	{
		public InvalidException(ValidationResult[] violations)
			: base(violations.Select(f => f.ToString()).Aggregate((lh, rh) => lh + ", " + rh))
		{
		}
	}
}
