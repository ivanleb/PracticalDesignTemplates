using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class ParallelAsyncPipeline<T>
    {
        private readonly ConcurrentQueue<T> _jobs = new ConcurrentQueue<T>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public void Enqueue(T job) => _jobs.Enqueue(job);

        public void OnStart(int degreeOfParallelism, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            Parallel.ForEach(
                new ConcurrentQueuePartitioner<T>(_jobs),
                new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism, CancellationToken = _cts.Token },
                (item, state) =>
                {
                    action(item);
                    if (_cts.IsCancellationRequested)
                        state.Break();
                });
        }

        public void Stop() => _cts.Cancel();
    }

    internal class ConcurrentQueueEnumerableWrapper<T> : IEnumerable<T>
    {
        internal ConcurrentQueueEnumerableWrapper(ConcurrentQueue<T> concurrentQueue)
        {
            ConcurrentQueue = concurrentQueue;
        }

        public ConcurrentQueue<T> ConcurrentQueue { get; private set; }

        public IEnumerator<T> GetEnumerator() => new ConcurrentQueueEnumeratorWrapper<T>(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class ConcurrentQueueEnumeratorWrapper<T> : IEnumerator<T>
    {
        private readonly ConcurrentQueueEnumerableWrapper<T> _queue;

        internal ConcurrentQueueEnumeratorWrapper(ConcurrentQueueEnumerableWrapper<T> queue)
        {
            _queue = queue;
        }

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_queue.ConcurrentQueue.TryDequeue(out T item))
            {
                Current = item;
                return true;
            }
            return false;
        }

        public void Reset() { }
        public void Dispose() { }
    }

    internal class ConcurrentQueuePartitioner<T> : Partitioner<T>
    {
        private readonly ConcurrentQueueEnumerableWrapper<T> _queue;

        internal ConcurrentQueuePartitioner(ConcurrentQueue<T> queue)
        {
            _queue = new ConcurrentQueueEnumerableWrapper<T>(queue);
        }

        public override bool SupportsDynamicPartitions => true;
        public override IList<IEnumerator<T>> GetPartitions(int partitionCount)
        {
            List<IEnumerator<T>> list = new List<IEnumerator<T>>(partitionCount);
            for (int i = 0; i < partitionCount; i++)
            {
                list.Add(new ConcurrentQueueEnumeratorWrapper<T>(_queue));
            }
            return list;
        }

        public override IEnumerable<T> GetDynamicPartitions() => _queue;
    }
}
