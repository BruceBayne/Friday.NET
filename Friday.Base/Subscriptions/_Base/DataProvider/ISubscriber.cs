using System;
using System.Threading.Tasks;

namespace Friday.Base.Subscriptions._Base.DataProvider
{
	public interface ISubscriber<in TFilterCriteria, out TActionArg> where TFilterCriteria : IEquatable<TFilterCriteria>
	{
		Task<IDisposable> Subscribe(TFilterCriteria criteria, Action<TActionArg> onNextAction,
			Action<Exception> errorAction);
	}
}