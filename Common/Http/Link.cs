using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Http
{
	public class Link
	{
		public Link(Uri href, string identifier, string title)
		{
			Href = href;
			Identifier = identifier;
			Title = title;
		}

		public Uri Href { get; }
		public String Identifier { get; }
		public String Title { get; }
	}
}
