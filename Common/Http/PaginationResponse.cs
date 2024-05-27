using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Http
{
	public class PaginatedResponse<TResults>
	{
		public PaginatedResponse(Uri selfUri, int totalCount, TResults results)
		{
			SelfUri = selfUri;
			Count = totalCount;
			Results = results;
		}

		public Uri? SelfUri { get; set; }
		public Uri? NextUri { get; set; }
		public Uri? PreviousUri { get; set; }
		public int Count { get; set; }
		public TResults Results { get; set; }
	}
}
