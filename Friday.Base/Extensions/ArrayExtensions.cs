using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global

namespace Friday.Base.Extensions
{
	public static class ArrayExtensions
	{
		[Pure]
		public static int[] IndexOfSequence(this byte[] haystack, byte[] needle)
		{
			return IndexOfSequence(haystack, needle, 0, haystack.Length);
		}
		[Pure]
		public static string ToHexString(this byte[] inputBytes)
		{
			if (inputBytes == null)
				return string.Empty;

			return string.Join(string.Empty, inputBytes.Select(t => t.ToString("x2")));
		}

		[Pure]
		public static int[] IndexOfSequence(this ArraySegment<byte> self, byte[] needle, int searchLimit = 0)
		{
			return IndexOfSequence(self.Array, needle, self.Offset, self.Count);
		}

		/// <summary>
		/// Searches in the haystack array for the given needle using the default equality operator and returns the index at which the needle starts.
		/// </summary>
		/// <param name="haystack">Sequence to operate on.</param>
		/// <param name="needle">Sequence to search for.</param>
		/// <param name="offset">Offset to start from</param>
		/// <param name="length">Length to check</param>
		/// <param name="searchLimit">Limit to number of max results</param>
		/// <returns>Indexes Array of the needle within the haystack limited to <see cref="searchLimit"/> or empty sequence.</returns>

		[Pure]
		public static int[] IndexOfSequence(this byte[] haystack, byte[] needle, int offset, int length, int searchLimit = 0)
		{
			var empty = new int[0];

			if (IsEmptyLocate(haystack, needle))
				return empty;


			var list = new List<int>();
			var limitLeft = searchLimit;
			for (var i = offset; i < offset + length; i++)
			{
				if (!IsMatch(haystack, i, needle))
					continue;

				list.Add(i);
				if (searchLimit != 0)
				{
					limitLeft--;
					if (limitLeft == 0)
						break;
				}
			}

			return list.Count == 0 ? empty : list.ToArray();
		}

		private static bool IsMatch(byte[] array, int position, byte[] candidate)
		{
			if (candidate.Length > (array.Length - position))
				return false;

			for (int i = 0; i < candidate.Length; i++)
				if (array[position + i] != candidate[i])
					return false;

			return true;
		}

		private static bool IsEmptyLocate(byte[] array, byte[] candidate)
		{
			return array == null
				   || candidate == null
				   || array.Length == 0
				   || candidate.Length == 0
				   || candidate.Length > array.Length;
		}
	}
}