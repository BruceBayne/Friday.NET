namespace Friday.ValueTypes
{
    public struct Percent
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

        public static bool operator >(Percent m1, Percent m2)
        {
            return m1.Value > m2.Value;
        }

        public static bool operator <(Percent m1, Percent m2)
        {
            return m1.Value < m2.Value;
        }
    }
}