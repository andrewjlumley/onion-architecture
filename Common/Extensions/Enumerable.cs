using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
	public static class EnumerableExtensionMethods
	{
		public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
		{
			while (source.Any())
			{
				yield return source.Take(chunksize);
				source = source.Skip(chunksize);
			}
		}

		public static bool ContainsAny<T>(this IEnumerable<T> source, T[] values)
		{
			bool contains = false;

			foreach (T value in values)
			{
				if (value == null)
					continue;

				if (source.Contains(value))
				{
					contains = true;
					break;
				}
			}

			return contains;
		}

		private static Random rnd = new Random();

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rnd.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		/// <summary> 
		/// Sort one collection based on keys defined in another 
		/// </summary> 
		public static IEnumerable<TResult> SortBy<TResult, TKey>(this IEnumerable<TResult> itemsToSort, IEnumerable<TKey> sortKeys, Func<TResult, TKey> matchFunc)
		{
			var missingKeys = itemsToSort.Select(f => matchFunc(f)).Except(sortKeys);
			return sortKeys.Concat(missingKeys).Join(itemsToSort, key => key, matchFunc, (key, iitem) => iitem);
		}

		/// <summary>
		/// Returns the first index of value in the list that meets a condition
		/// </summary>
		public static int IndexOf<T>(this IEnumerable<T> list, Predicate<T> condition)
		{
			int i = -1;
			return list.Any(x => { i++; return condition(x); }) ? i : -1;
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			return enumerable == null || !enumerable.Any();
		}

		public static bool IsOnlyOne<T>(this IEnumerable<T> list)
		{
			return list.Count() == 1;
		}

		public static bool IsMoreThanOne<T>(this IEnumerable<T> list)
		{
			return list.Count() > 1;
		}

		public static bool TryHasResults<T>(this IEnumerable<T> source, Func<T, bool> predicate, out IEnumerable<T> results)
		{
			results = source.Where(predicate);
			return results.Any();
		}
	}
}
