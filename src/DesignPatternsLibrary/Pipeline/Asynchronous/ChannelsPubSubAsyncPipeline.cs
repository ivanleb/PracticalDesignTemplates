using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class ChannelsPubSubAsyncPipeline<DataType>
    {
        private ChannelWriter<IContext<DataType>> _writer;
        private Dictionary<Type, Action<IContext<DataType>>> _handlers = new Dictionary<Type, Action<IContext<DataType>>>();

        public ChannelsPubSubAsyncPipeline()
        {
            Channel<IContext<DataType>> channel = Channel.CreateUnbounded<IContext<DataType>>();
            ChannelReader<IContext<DataType>> reader = channel.Reader;
            _writer = channel.Writer;

            Task.Factory.StartNew(async () =>
            {
                while (await reader.WaitToReadAsync())
                {
                    IContext<DataType> job = await reader.ReadAsync();
                    if (_handlers.TryGetValue(job.GetType(), out Action<IContext<DataType>> value))
                    {
                        value.Invoke(job);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void RegisterHandler<T>(Action<T> handleAction) where T : IContext<DataType>
        {
            Action<IContext<DataType>> actionWrapper = (job) => handleAction((T)job);
            _handlers.Add(typeof(T), actionWrapper);
        }

        public async Task Enqueue(IContext<DataType> job) => await _writer.WriteAsync(job);

        public void Stop() => _writer.Complete();
    }
}
