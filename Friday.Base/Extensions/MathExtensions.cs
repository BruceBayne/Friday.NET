using System;

namespace Friday.Base.Extensions
{
	public static class MathExtensions
	{
		public static T Min<T>(T one, T two) where T : struct, IComparable<T>
		{
			return one.CompareTo(two) < 0 ? one : two;
		}


		public static T Max<T>(T one, T two) where T : struct, IComparable<T>
		{
			return one.CompareTo(two) < 0 ? two : one;
		}
	}
}