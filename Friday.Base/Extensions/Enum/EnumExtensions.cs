using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Friday.Base.Extensions.Enum
{
    public static class EnumExtensions
    {


        private static System.Random GetRandom(int seed = 0)
        {
            if (seed == 0)
                seed = Environment.TickCount;
            return new System.Random(seed);
        }

        public static T RandomValue<T>(this T enumeratedType, int seed = 0) where T : struct
        {
            var v = System.Enum.GetValues(typeof(T));
            return (T)v.GetValue(GetRandom(seed).Next(v.Length));
        }


        public static T RandomValue<T>(this T enumeratedType, IEnumerable<string> allowed, int seed = 0) where T : struct
        {
            var v = System.Enum.GetValues(typeof(T));
            var arr = v.Cast<T>().Where(x => allowed.Contains(x.ToString())).ToArray();
            return (T)arr.GetValue(GetRandom(seed).Next(arr.Length));
        }



        public static bool ContainExactFlagSet(this System.Enum obj, System.Enum flags)
        {
            var objFlags = obj.EnumerateSettedUpFlags().ToList();
            var setFlags = flags.EnumerateSettedUpFlags().ToList();

            if (objFlags.Count != setFlags.Count)
                return false;

            foreach (var tmpFlag in objFlags)
            {
                if (!setFlags.Contains(tmpFlag))
                    return false;

            }
            return true;

        }


        public static IEnumerable<System.Enum> EnumerateSettedUpFlags(this System.Enum flags)
        {
            foreach (var value in System.Enum.GetValues(flags.GetType()).Cast<System.Enum>())
            {

                ulong num = Convert.ToUInt64(value);

                var hasFlag = ((Convert.ToUInt64(flags) & num) == num);

                if (hasFlag)
                    yield return value;
            }
        }

        public static IEnumerable<System.Enum> EnumerateAllFlags(this System.Enum flags)
        {
            foreach (var value in System.Enum.GetValues(flags.GetType()).Cast<System.Enum>())
            {
                yield return value;
            }
        }



        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
            var arr = (T[])System.Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(arr, src) + 1;
            return arr.Length == j ? arr[0] : arr[j];
        }


        public static bool HasFlag<TEnum>(this TEnum enumeratedType, TEnum value)
            where TEnum : struct, IComparable, IFormattable, IConvertible

        {
            if (!(enumeratedType is System.Enum))
            {
                throw new InvalidOperationException("Struct is not an Enum.");
            }

            if (typeof(TEnum).GetCustomAttributes(
                typeof(FlagsAttribute), false).Length == 0)
            {
                throw new InvalidOperationException("Enum must use [Flags].");
            }

            long enumValue = enumeratedType.ToInt64(CultureInfo.InvariantCulture);
            long flagValue = value.ToInt64(CultureInfo.InvariantCulture);

            if ((enumValue & flagValue) == flagValue)
            {
                return true;
            }

            return false;
        }
    }
}