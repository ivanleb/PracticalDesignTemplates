using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.EventLoop
{
    public class EventLoop<T>
    {
        private readonly BufferBlock<T> _queue;
        private readonly CancellationToken _cancelationToken;
        private readonly TimeSpan _loopInterval;

        public EventLoop(CancellationToken cancelationToken, TimeSpan loopInterval)
        {
            DataflowBlockOptions dtOptions = new DataflowBlockOptions { CancellationToken = cancelationToken };
            _queue = new BufferBlock<T>(dtOptions);
            _cancelationToken = cancelationToken;
            _loopInterval = loopInterval;
        }

        public void Push(T item) => _queue.Post(item);

        public async Task Run(Action<T> processMessage, Action noMessagesInTimeout)
        {
            if(processMessage == null) 
                throw new ArgumentNullException(nameof(processMessage));
            if (noMessagesInTimeout == null)
                throw new ArgumentNullException(nameof(noMessagesInTimeout));

            while (true)
            {
                T msg;
                try
                {
                    msg = await _queue.ReceiveAsync(_loopInterval, _cancelationToken);
                }
                catch (TimeoutException)
                {
                    noMessagesInTimeout();
                    continue;
                }
                catch (Exception e)
                {
                    break;
                }
                processMessage(msg);    
            }
        }
    }
}
