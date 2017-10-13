using System;

namespace Friday.ValueTypes.Currencies
{
    public static class BitCoinExtensions
    {
        public static BitCoin TakePercent(this BitCoin b, Percent p)
        {
            decimal onePercentInSatoshi = (b.SatoshiAmount / 100m);
            var totalSatoshi = onePercentInSatoshi * p.Value;


            return BitCoin.FromSatoshi((ulong) Math.Floor(totalSatoshi));
        }
    }
}