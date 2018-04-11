using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Friday.Base.ValueTypes
{
	[Serializable]
	public struct Percent : IComparable<Percent>, ISerializable
	{
		public readonly decimal Value;

		private Percent(decimal percent)
		{
			Value = percent;
		}


		/// <summary>
		/// 0% Percent
		/// </summary>
		public static Percent Zero => From(0);


		/// <summary>
		/// 1% Percent
		/// </summary>
		public static Percent One => From(1);

		/// <summary>
		/// 10% Percents
		/// </summary>
		public static Percent Ten => From(10);


		/// <summary>
		/// 50% Percents
		/// </summary>
		public static Percent Fifty => From(50);

		public override string ToString()
		{
			return $"{nameof(Value)}: {Value}";
		}

		private Percent(SerializationInfo info, StreamingContext para)
		{
			Value = info.GetDecimal(nameof(Value));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			info.AddValue(nameof(Value), Value);
		}

		/// <summary>
		/// 100% Percents
		/// </summary>
		public static Percent Hundred => From(100);


		public static Percent From(byte value)
		{
			return new Percent(value);
		}


		public static Percent From(decimal value)
		{
			return new Percent(value);
		}

		public static decimal CalculatePercentAmountFromValue(decimal value, Percent percent)
		{
			return value * percent.Value / 100;
		}

		public static int CalculatePercentAmountFromValue(int value, Percent percent)
		{
			return (int)Math.Round(value * percent.Value / 100);
		}

		public static long CalculatePercentAmountFromValue(long value, Percent percent)
		{
			return (long)Math.Round(value * percent.Value / 100);
		}

		public static uint CalculatePercentAmountFromValue(uint value, Percent percent)
		{
			return (uint)Math.Round(value * percent.Value / 100);
		}

		public static ulong CalculatePercentAmountFromValue(ulong value, Percent percent)
		{
			return (ulong)Math.Round(value * percent.Value / 100);
		}

		public static int operator +(int m1, Percent m2)
		{
			return m1 + CalculatePercentAmountFromValue(m1, m2);
		}

		public static long operator +(long m1, Percent m2)
		{
			return m1 + CalculatePercentAmountFromValue(m1, m2);
		}

		public static uint operator +(uint m1, Percent m2)
		{
			return m1 + CalculatePercentAmountFromValue(m1, m2);
		}

		public static ulong operator +(ulong m1, Percent m2)
		{
			return m1 + CalculatePercentAmountFromValue(m1, m2);
		}

		public static decimal operator +(decimal m1, Percent m2)
		{
			return m1 + CalculatePercentAmountFromValue(m1, m2);
		}

		public static int operator -(int m1, Percent m2)
		{
			return m1 - CalculatePercentAmountFromValue(m1, m2);
		}

		public static long operator -(long m1, Percent m2)
		{
			return m1 - CalculatePercentAmountFromValue(m1, m2);
		}

		public static uint operator -(uint m1, Percent m2)
		{
			return m1 - CalculatePercentAmountFromValue(m1, m2);
		}

		public static ulong operator -(ulong m1, Percent m2)
		{
			return m1 - CalculatePercentAmountFromValue(m1, m2);
		}

		public static decimal operator -(decimal m1, Percent m2)
		{
			return m1 - CalculatePercentAmountFromValue(m1, m2);
		}

		public static Percent operator +(Percent m1, Percent m2)
		{
			return From(m1.Value + m2.Value);
		}

		public static Percent operator -(Percent m1, Percent m2)
		{
			return From(m1.Value - m2.Value);
		}

		public static bool operator >(Percent m1, Percent m2)
		{
			return m1.Value > m2.Value;
		}

		public static bool operator <(Percent m1, Percent m2)
		{
			return m1.Value < m2.Value;
		}

		public static bool operator <=(Percent m1, Percent m2)
		{
			return m1.Value <= m2.Value;
		}

		public static bool operator >=(Percent m1, Percent m2)
		{
			return m1.Value >= m2.Value;
		}

		public static bool operator ==(Percent m1, Percent m2)
		{
			return m1.Value == m2.Value;
		}

		public static bool operator !=(Percent m1, Percent m2)
		{
			return m1.Value != m2.Value;
		}

		[Pure]
		public int CompareTo(Percent other)
		{
			return Value.CompareTo(other.Value);
		}

		[Pure]
		public bool Equals(Percent other)
		{
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Percent percent && Equals(percent);
		}

		[Pure]
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}