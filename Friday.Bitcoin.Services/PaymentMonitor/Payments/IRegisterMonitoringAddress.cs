using NBitcoin;

namespace Friday.Bitcoin.Services.PaymentMonitor.Payments
{
	public interface IRegisterMonitoringAddress
	{
		bool RegisterMonitoringAddress(MonitoringBlock lku);
		bool RegisterMonitoringAddress(BitcoinAddress bitcoinAddress);
	}
}