using System;
using System.Collections.Generic;

namespace Friday.ValueTypes.Currencies
{
    public struct BitCoin : IEqualityComparer<BitCoin>, IComparable<BitCoin>
    {
        public const uint BitsInOneBtc = 1_000_000;
        public const uint SatoshiInOneBtc = 100_000_000;

        public readonly ulong SatoshiAmount;



        public decimal ToBtc()
        {

            return (decimal)SatoshiAmount / (decimal)SatoshiInOneBtc;
        }


        public override string ToString()
        {

            var formatted = SatoshiAmount.ToString("N0");


            return $"{nameof(SatoshiAmount)}: {formatted}";
        }

       public static BitCoin Zero => new BitCoin(0);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BitCoin && Equals((BitCoin)obj);
        }

        public override int GetHashCode()
        {
            return SatoshiAmount.GetHashCode();
        }

        public static implicit operator ulong(BitCoin x)
        {
            return x.SatoshiAmount;
        }

        private BitCoin(ulong satoshiAmount)
        {
            SatoshiAmount = satoshiAmount;
        }


        public static BitCoin One => FromBtc(1);


        public static BitCoin FromBtc(ushort btcAmount)
        {
            checked
            {
                ulong satoshiAmount = btcAmount * SatoshiInOneBtc;
                return new BitCoin(satoshiAmount);
            }

        }
        public static BitCoin FromBits(uint bits)
        {
            checked
            {
                ulong satoshiAmount = 100U * bits;
                return new BitCoin(satoshiAmount);
            }
        }

        public static BitCoin FromSatoshi(ulong satoshiAmount)
        {
            return new BitCoin(satoshiAmount);
        }
        public BitCoin Add(BitCoin other)
        {
            checked
            {
                return new BitCoin(SatoshiAmount + other.SatoshiAmount);
            }
        }
        public BitCoin Subtract(BitCoin other)
        {
            checked
            {
                return new BitCoin(SatoshiAmount - other.SatoshiAmount);
            }

        }

        public static BitCoin operator +(BitCoin m1, BitCoin m2)
        {
            return m1.Add(m2);
        }
        public static BitCoin operator -(BitCoin m1, BitCoin m2)
        {
            return m1.Subtract(m2);
        }



        public static bool operator ==(BitCoin m1, BitCoin m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator !=(BitCoin m1, BitCoin m2)
        {
            return !(m1 == m2);
        }

        public static bool operator >(BitCoin m1, BitCoin m2)
        {
            return m1.SatoshiAmount > m2.SatoshiAmount;
        }


        public static bool operator <(BitCoin m1, BitCoin m2)
        {
            return m1.SatoshiAmount < m2.SatoshiAmount;
        }


        public bool Equals(BitCoin other)
        {
            return SatoshiAmount == other.SatoshiAmount;
        }

        public int CompareTo(BitCoin other)
        {
            if (SatoshiAmount < other.SatoshiAmount) return -1;
            return SatoshiAmount == other.SatoshiAmount ? 0 : 1;
        }

        public bool Equals(BitCoin x, BitCoin y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(BitCoin obj)
        {
            return (int)(SatoshiAmount ^ (SatoshiAmount >> 32));
        }
    }
}