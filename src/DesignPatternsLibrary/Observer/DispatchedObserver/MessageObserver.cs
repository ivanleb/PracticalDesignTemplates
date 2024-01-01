using System;

namespace DesignPatternsLibrary.Observer.DispatchedObserver
{
    public class MessageObserver<T> : IObserver<T>
    {
        public void OnCompleted()
        {
            Console.WriteLine($"OnCompleted {DateTime.Now}");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"OnError {DateTime.Now}, error: {error.Message}");
        }

        public void OnNext(T value)
        {
            Console.WriteLine($"OnNext {DateTime.Now}, message: {value} ");
        }
    }
}
