using Friday.Base.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Friday.ValueTypes.Currencies
{
	public static class BitCoinExtensions
	{

		// ReSharper disable once InconsistentNaming
		public static BitCoin BTC(this int bitCoinAmount)
		{
			if (bitCoinAmount > ushort.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(bitCoinAmount));
			}
			return BitCoin.FromBtc((ushort)bitCoinAmount);

		}




		public static BitCoin TakePercent(this BitCoin b, Percent p)
		{
			decimal onePercentInSatoshi = (b.SatoshiAmount / 100m);
			var totalSatoshi = onePercentInSatoshi * p.Value;
			return BitCoin.FromSatoshi((ulong)Math.Floor(totalSatoshi));
		}

		public static BitCoin Sum<T>(this IEnumerable<T> z, Func<T, BitCoin> selector)
		{
			return z.Aggregate(BitCoin.Zero, (current, coin) => current + selector(coin));
		}
	}
}