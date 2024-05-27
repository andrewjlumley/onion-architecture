using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Http
{
	public class RestUri
	{
		public RestUri(Uri uri, HttpMethod action)
		{
			Href = uri;
			Action = action.ToString();
		}

		public Uri Href { get; private set; }
		public String Action { get; private set; }
	}
}
