using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Extensions
{
	public static class StringExtensionMethods
	{
		/// <summary>
		/// Take a long string and then split it based on a maximum length, taking into account spaces and never splitting a word.
		/// </summary>
		public static IEnumerable<string> SplitWithSpaces(this string input, int maxLength)
		{
			var source = input;
			while (source != string.Empty)
			{
				var lineMaxLength = source.Length > maxLength ? maxLength : source.Length;
				var index = source.IndexOf(Environment.NewLine, 0, lineMaxLength);
				if (index == -1 && source.Length > maxLength)
					index = source.LastIndexOf(' ', lineMaxLength - 1);
				if (index == -1)
					index = lineMaxLength;

				yield return source.Substring(0, index).Trim(' ', '\r', '\n');
				source = source.Substring(index).TrimStart(' ', '\r', '\n');
			}
		}

		public static bool Contains(this string input, string value, StringComparison comparisonType)
		{
			return input.IndexOf(value, comparisonType) >= 0;
		}

		public static bool ContainsAny(this string str, string[] values)
		{
			if (values == null || values.Length == 0)
				return false;

			return values.Any(f => str.Contains(f));
		}

		public static string FlattenToString(this string[] input)
		{
			if (input.IsNullOrEmpty())
				return string.Empty;

			var result = input.Where(f => string.IsNullOrWhiteSpace(f) == false).Aggregate((lh, rh) => lh + ", " + rh);

			var lastComma = result.LastIndexOf(",");
			if (lastComma != -1)
				result = result.Remove(lastComma, 1).Insert(lastComma, " and");

			return result.TrimEnd();
		}

		public static string Ellipsis(this string input, int charsToDisplay)
		{
			if (string.IsNullOrWhiteSpace(input))
				return string.Empty;

			if (input.Length <= charsToDisplay)
				return input;

			return new string(input.Take(charsToDisplay).ToArray()) + "...";
		}

		public static String? TrimToLength(this String str, int maxLength)
		{
			if (str.IsNullOrEmpty())
				return str;

			if (str.Length > maxLength)
				return str.Substring(0, maxLength);

			return str;
		}

		/// <summary>
		/// Returns string contained between two strings of original string
		/// </summary>
		public static String Between(this string originalString, string string1, string string2)
		{
			int pFrom = originalString.IndexOf(string1) + string1.Length;
			int pTo = originalString.LastIndexOf(string2);
			return originalString.Substring(pFrom, pTo - pFrom);
		}

		/// <summary>
		/// Returns a boolean value indicating whether, or not, the string starts with any of the specified values.
		/// </summary>
		public static bool StartsWith(this string str, string[] values)
		{
			bool startsWith = false;

			foreach (string value in values)
			{
				if (string.IsNullOrEmpty(value))
					continue;

				if (str.StartsWith(value))
				{
					startsWith = true;
					break;
				}
			}

			return startsWith;
		}

		public static String? CapitalisedFirstLetter(this String text)
		{
			if (text.IsNullOrEmpty())
				return text;

			return char.ToUpper(text[0]) + text.Substring(1);
		}

		public static String ToTitleCase(this String text)
		{
			if (String.IsNullOrEmpty(text) == true)
				return text;

			CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
			TextInfo textInfo = cultureInfo.TextInfo;
			return textInfo.ToTitleCase(text);
		}

		public static String GenerateHMAC256Signature(this string str, String key)
		{
			using (KeyedHashAlgorithm kha = new HMACSHA256(ASCIIEncoding.UTF8.GetBytes(key)))
			{
				byte[] hashSourceBytes = ASCIIEncoding.UTF8.GetBytes(str);
				byte[] hashOutputBytes = kha.ComputeHash(hashSourceBytes);
				return Convert.ToBase64String(hashOutputBytes);
			}
		}
	}
}
