using System;

namespace Friday.Bitcoin.Services.PaymentMonitor.Payments
{
	public interface IMoneyAmountChanged
	{
		event EventHandler<MoneyEventInfo> OnAmountChanged;
	}
}