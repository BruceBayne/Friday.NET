namespace Friday.Base.Serialization
{

    public interface ICompleteSerializer : ISerializer , IDeserializer
    {
        

    }

    public interface ISerializer
    {
        byte[] Serialize(object packet);
    }
}