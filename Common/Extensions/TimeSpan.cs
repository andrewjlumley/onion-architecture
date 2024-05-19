using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
	public static class TimeSpanExtensionMethods
	{
		public static bool IsWithin(this TimeSpan span, TimeSpan earliest, TimeSpan latest)
		{
			return span >= earliest && span <= latest;
		}

		public static bool IsInFuture(this TimeSpan ts)
		{
			return ts > TimeSpan.Zero;
		}

		public static bool IsInPast(this TimeSpan ts)
		{
			return ts < TimeSpan.Zero;
		}

		public static String AsFormatted(this TimeSpan ts)
		{
			var description = String.Empty;

			if (ts == TimeSpan.Zero)
				description = "Nothing";

			if (ts.Days > 0)
				description += (ts.Days + " day" + (ts.Days > 1 ? "s" : String.Empty) + ", ");

			if (ts.Hours > 0)
				description += (ts.Hours + " hour" + (ts.Hours > 1 ? "s" : String.Empty) + ", ");

			if (ts.Minutes > 0)
				description += (ts.Minutes + " minute" + (ts.Minutes > 1 ? "s" : String.Empty) + ", ");

			return description.TrimEnd(new char[] { ',', ' ' });
		}
	}
}
