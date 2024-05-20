using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
	public interface IDomainObject
	{
		void RegisterTarget(Type type, IDomainObject target);
		bool TryIsValid(out ValidationResult[] violations);
	}
}
