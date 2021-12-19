using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public interface IJob<T> 
    {
        T JobData { get; }
    }

    public class ChannelsImproovedAsyncPipeline<JobDataType>
    {
        private ChannelWriter<IJob<JobDataType>> _writer;
        private Dictionary<Type, Action<IJob<JobDataType>>> _handlers = new Dictionary<Type, Action<IJob<JobDataType>>>();

        public ChannelsImproovedAsyncPipeline()
        {
            Channel<IJob<JobDataType>> channel = Channel.CreateUnbounded<IJob<JobDataType>>();
            ChannelReader<IJob<JobDataType>> reader = channel.Reader;
            _writer = channel.Writer;

            Task.Factory.StartNew(async () =>
            {
                while (await reader.WaitToReadAsync())
                {
                    IJob<JobDataType> job = await reader.ReadAsync();
                    if (_handlers.TryGetValue(job.GetType(), out Action<IJob<JobDataType>> value))
                    {
                        value.Invoke(job);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void RegisterHandler<T>(Action<T> handleAction) where T : IJob<JobDataType>
        {
            Action<IJob<JobDataType>> actionWrapper = (job) => handleAction((T)job);
            _handlers.Add(typeof(T), actionWrapper);
        }

        public async Task Enqueue(IJob<JobDataType> job) => await _writer.WriteAsync(job);

        public void Stop() => _writer.Complete();
    }
}
