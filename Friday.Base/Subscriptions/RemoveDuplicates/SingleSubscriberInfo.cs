using System;
using Friday.Base.Subscriptions._Base;

namespace Friday.Base.Subscriptions.RemoveDuplicates
{
	/// <summary>
	/// Contains reactive actions
	/// </summary>
	/// <typeparam name="TNextArg"></typeparam>
	public struct SingleSubscriberInfo<TNextArg> : IEquatable<SingleSubscriberInfo<TNextArg>>
	{
		public readonly ReactiveActions<TNextArg> Actions;

		public SingleSubscriberInfo(ReactiveActions<TNextArg> actions)
		{
			Actions = actions;
		}


		public void CallOnNext(TNextArg argument)
		{
			Actions.NextAction?.Invoke(argument);
		}

		public void CallOnError(Exception exception)
		{
			Actions.ErrorAction?.Invoke(exception);
		}

		public bool Equals(SingleSubscriberInfo<TNextArg> other)
		{
			return Equals(Actions, other.Actions);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is SingleSubscriberInfo<TNextArg> info && Equals(info);
		}

		public override int GetHashCode()
		{
			return (Actions != null ? Actions.GetHashCode() : 0);
		}
	}
}