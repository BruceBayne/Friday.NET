using System;

namespace Friday.Bitcoin.Services.PaymentMonitor.Payments
{
    public class PaymentMonitorSettings
    {
        public TimeSpan RefreshInterval = TimeSpan.FromMinutes(10);
        public Uri ApiUrl = new Uri("https://api.qbit.ninja/");
        public int MinConfirmations = 3;
    }
}