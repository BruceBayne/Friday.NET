namespace Friday.Base.Tasks.CachedTask
{
	public enum CacheFallbackPolicy
	{
		Invalid,
		SetExceptionOnFail,
		LeavePreviousSuccessValue,
	}
}