using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
	internal interface IDomainObject
	{
		bool TryIsValid(out ValidationResult[] violations);
	}
}
