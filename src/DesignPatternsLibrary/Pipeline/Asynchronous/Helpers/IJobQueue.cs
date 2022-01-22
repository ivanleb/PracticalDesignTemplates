namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public interface IJobQueue<T>
    {
        void Enqueue(T item);
    }
}
