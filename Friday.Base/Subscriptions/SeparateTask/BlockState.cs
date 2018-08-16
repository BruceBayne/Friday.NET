namespace Friday.Base.Subscriptions.SeparateTask
{
	public enum BlockState
	{
		Invalid,


		SubscribeRequired,
		Alive,
		UnsubscribeRequired,
		CleanupRequired,
	}
}