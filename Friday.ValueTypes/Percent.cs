using System;

namespace Friday.ValueTypes
{
    public struct Percent : IComparable<Percent>
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

        public static decimal Calculate(decimal value, Percent percent)
        {
            return value * percent.Value/100;
        }

        public static int Calculate(int value, Percent percent)
        {
            return (int) Math.Round(value * percent.Value/100);
        }

        public static long Calculate(long value, Percent percent)
        {
            return (long)Math.Round(value * percent.Value/100);
        }

        public static int operator +(int m1, Percent m2)
        {
            return m1 + Calculate(m1, m2);
        }

        public static long operator +(long m1, Percent m2)
        {
            return m1 + Calculate(m1, m2);
        }

        public static decimal operator +(decimal m1, Percent m2)
        {
            return m1 + Calculate(m1, m2);
        }

        public static int operator -(int m1, Percent m2)
        {
            return m1 - Calculate(m1, m2);
        }

        public static long operator -(long m1, Percent m2)
        {
            return m1 - Calculate(m1, m2);
        }

        public static decimal operator -(decimal m1, Percent m2)
        {
            return m1 - Calculate(m1, m2);
        }

        public static Percent operator +(Percent m1, Percent m2)
        {
            return Percent.From(m1.Value + m2.Value);
        }

        public static Percent operator -(Percent m1, Percent m2)
        {
            return Percent.From(m1.Value - m2.Value);
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

        public int CompareTo(Percent other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}