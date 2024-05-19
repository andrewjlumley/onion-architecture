using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
	public static class NumericExtensionMethods
	{
		/// <summary>
		/// Convert to ordinal, e.g. 1 becomes 1st, 2 becomes 2nd.
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string Ordinal(this int number)
		{
			// http://stackoverflow.com/questions/20156/is-there-an-easy-way-to-create-ordinals-in-c
			const string TH = "th";
			var s = number.ToString();

			number %= 100;

			if ((number >= 11) && (number <= 13))
			{
				return s + TH;
			}

			switch (number % 10)
			{
				case 1:
					return s + "st";
				case 2:
					return s + "nd";
				case 3:
					return s + "rd";
				default:
					return s + TH;
			}
		}

		public static bool HasEvenCheckbit(this ulong input)
		{
			ulong count = 0;
			for (int i = 0; i < 64; i++) count += input >> i & 1;

			return count % 2 == 0;
		}

		public static double RoundUp(this double value, double multiple)
		{
			double delta = (multiple - (value % multiple)) % multiple;
			return value + delta;
		}

		public static double RoundDown(this double value, double multiple)
		{
			double delta = value % multiple;
			return value - delta;
		}

		public static double RoundToNearest(this double value, double multiple)
		{
			double delta = value % multiple;
			bool roundUp = delta > multiple / 2;

			return roundUp ? value.RoundUp(multiple) : value.RoundDown(multiple);
		}
	}
}
