using System;
using System.Threading;

namespace DesignPatternsLibrary.Observer.DispatchedObserver
{
    public class ProcessingMessageDispatcher<T> : CDispatcherBase<T>
    {
        private Thread _dispatchThread;

        public static ProcessingMessageDispatcher<T> Create(TimeSpan dispatchInterval)
            => new ProcessingMessageDispatcher<T>(new ObservableSubject<T>(), dispatchInterval);
        

        public ProcessingMessageDispatcher(ObservableSubject<T> observableSubject, TimeSpan dispatchInterval) 
            : base(observableSubject, dispatchInterval)
        {}

        public override void Dispatch()
        {
            if (_dispatchThread != null)
                return;

            cancel = true;
            _dispatchThread = new Thread(() => DoDispatch());
            _dispatchThread.Start();
        }
    }
}
