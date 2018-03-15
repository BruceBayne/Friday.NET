using System.Threading.Tasks;

namespace Friday.Base
{
	public interface IStartableAsyncMarker
	{
		Task StartAsync();
	}
}