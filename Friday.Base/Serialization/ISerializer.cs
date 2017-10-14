namespace Friday.Base.Serialization
{
    public interface ISerializer
    {
        byte[] Serialize(object packet);
    }
}