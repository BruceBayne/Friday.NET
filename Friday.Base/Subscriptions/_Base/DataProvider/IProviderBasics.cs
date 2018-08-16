using System.Threading.Tasks;

namespace Friday.Base.Subscriptions._Base.DataProvider
{
	public interface IProviderBasics : IDataProviderEvents
	{
		Task Connect();
		void Disconnect();
	}
}