namespace Friday.Base.Patterns.Memento
{
    /// <summary>
    /// Represent Memento Pattern for saving object state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMementoGetter<out T>
    {
        T CreateMemento();
    }
}