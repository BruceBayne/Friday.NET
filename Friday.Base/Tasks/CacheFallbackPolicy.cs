namespace Friday.Base.Tasks
{
	public enum CacheFallbackPolicy
	{
		Invalid,
		SetExceptionOnFail,
		LeavePreviousSuccessValue,
	}
}