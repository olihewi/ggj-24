using System.Collections.Generic;
using Random = System.Random;

namespace Utilities
{
	public static class ListUtils
	{
		private static readonly Random _Random = new();

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = _Random.Next(n + 1);
				(list[k], list[n]) = (list[n], list[k]);
			}
		}
	}
}
