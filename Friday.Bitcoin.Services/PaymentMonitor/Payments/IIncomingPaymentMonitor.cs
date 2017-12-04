namespace Friday.Bitcoin.Services.PaymentMonitor.Payments
{
	public interface IIncomingPaymentMonitor : IRegisterMonitoringAddress, IMoneyAmountChanged
	{
	}
}