namespace Friday.Base.Patterns.Memento
{
    /// <summary>
    /// Represent Memento Pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMementoSetter<in T>
    {
        void SetMemento(T memento);
    }
}