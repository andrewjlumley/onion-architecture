using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
	public static class EnumHelper
	{
		public static T? GetAttributeOfType<T>(this Enum value) where T : System.Attribute
		{
			var type = value.GetType();
			var memInfo = type.GetMember(value.ToString());
			var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
			return attributes.Any() ? (T)attributes.First() : null;
		}

		public static T[] GetEnumList<T>()
		{
			var test = Enum.GetValues(typeof(T));
			return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
		}
	}
}
