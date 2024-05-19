using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
	public sealed class NotifiedByAttribute : Attribute
	{
		public NotifiedByAttribute(params string[] properties)
		{
			Properties = properties;
		}

		public string[] Properties { get; private set; }

		public override string ToString()
		{
			return "Properties: " + String.Join(", ", Properties);
		}
	}
}
