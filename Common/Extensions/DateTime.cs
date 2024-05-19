using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
	public static class DateTimeExtensionMethods
	{
		#region Conditional

		public static bool IsWeekend(this DateTime dateTime)
		{
			return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
		}

		public static bool IsWithin(this DateTime dateTime, DateTime earliest, DateTime latest)
		{
			return dateTime >= earliest && dateTime <= latest;
		}

		public static bool IsWithinWeekCommencing(this DateTime dateTime, DateTime weekCommencing)
		{
			return dateTime >= weekCommencing && dateTime <= weekCommencing.AddDays(6);
		}

		/// <summary>
		/// True if the year is after 1979 and before 2079.
		/// </summary>
		public static bool IsSensible(this DateTime dateTime)
		{
			return dateTime.Year > 1979 && dateTime.Year < 2079;
		}

		#endregion

		#region Comparison

		public static int MonthsDifference(this DateTime lh, DateTime rh)
		{
			return (lh.Month - rh.Month) + (12 * (lh.Year - rh.Year));
		}

		#endregion

		#region Nudge date

		public static DateTime ValidSqlDateTime(this DateTime dateTime)
		{
			DateTime minimum = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
			if (minimum > dateTime)
				return minimum;

			DateTime maximum = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;
			if (dateTime > maximum)
				return maximum;

			// Remove miliseconds from date and time.  Useful where issue with SQL precision on Datetime data type
			return dateTime.AddMilliseconds(-dateTime.Millisecond);
		}

		public static DateTime NextWeekDay(this DateTime dateTime, DayOfWeek day)
		{
			DateTime result = dateTime.AddDays(1);

			while (result.DayOfWeek != day)
				result = result.AddDays(1);

			return result;
		}

		public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek)
		{
			int diff = dateTime.DayOfWeek - startOfWeek;
			if (diff < 0)
			{
				diff += 7;
			}
			return dateTime.AddDays(-1 * diff).Date;
		}

		public static DateTime AddWeeks(this DateTime dateTime, long value)
		{
			DateTime result = dateTime;
			for (uint week = 0; week < Math.Abs(value); week++)
				result = result.AddDays(value > 0 ? 7 : -7);

			return result;
		}

		#endregion

		#region Rounding

		public static DateTime RoundSecond(this DateTime dateTime)
		{
			DateTime rounded = dateTime.Date + TimeSpan.FromSeconds(Math.Round(dateTime.TimeOfDay.TotalSeconds));
			return rounded;
		}

		public static DateTime RoundMinute(this DateTime dateTime)
		{
			DateTime rounded = dateTime.Date + TimeSpan.FromMinutes(Math.Round(dateTime.TimeOfDay.TotalMinutes));
			return rounded;
		}

		public static DateTime RoundHour(this DateTime dateTime)
		{
			DateTime rounded = dateTime.Date + TimeSpan.FromHours(Math.Round(dateTime.TimeOfDay.TotalHours));
			return rounded;
		}

		public static DateTime RoundUp(this DateTime dateTime, TimeSpan timespan)
		{
			var delta = (timespan.Ticks - (dateTime.Ticks % timespan.Ticks)) % timespan.Ticks;
			return new DateTime(dateTime.Ticks + delta);
		}

		public static DateTime RoundDown(this DateTime dateTime, TimeSpan timespan)
		{
			var delta = dateTime.Ticks % timespan.Ticks;
			return new DateTime(dateTime.Ticks - delta);
		}

		public static DateTime RoundToNearest(this DateTime dateTime, TimeSpan timespan)
		{
			var delta = dateTime.Ticks % timespan.Ticks;
			bool roundUp = delta > timespan.Ticks / 2;

			return roundUp ? dateTime.RoundUp(timespan) : dateTime.RoundDown(timespan);
		}

		#endregion

		#region Presentation

		public static String AsFormatted(this DateTime dateTime)
		{
			if (dateTime.TimeOfDay.TotalMinutes == 0)
				return dateTime.Date == DateTime.Now.Date ? "today" : dateTime.ToString("d-MMM-yyyy");
			else if (dateTime.Date == DateTime.Now.Date)
				return "today at " + dateTime.ToString("HH:mm");
			else
				return dateTime.ToString("d-MMM-yyyy") + " at " + dateTime.ToString("HH:mm");
		}

		#endregion
	}
}
