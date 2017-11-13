namespace Friday.Base.Patterns.Memento
{
    /// <summary>
    /// Represent Memento Pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMementoHolder<T> : IMementoGetter<T>, IMementoSetter<T>
    {

    }
}