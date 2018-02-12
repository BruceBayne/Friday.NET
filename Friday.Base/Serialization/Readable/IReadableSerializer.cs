namespace Friday.Base.Serialization.Readable
{
	public interface IReadableSerializer
	{
		string Serialize(object packet);
	}
}