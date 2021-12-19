using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class ChannelsMultiThreadsAsyncPipeline
    {
        private readonly ChannelWriter<Action> _writer;

        public ChannelsMultiThreadsAsyncPipeline(int threadCount)
        {
            Channel<Action> channel = Channel.CreateUnbounded<Action>();
            ChannelReader<Action> reader = channel.Reader;
            _writer = channel.Writer;
            for (int i = 0; i < threadCount; i++)
            {
                int threadId = i;
                Task.Factory.StartNew(async () =>
                {
                    while (await reader.WaitToReadAsync())
                    {
                        Action job = await reader.ReadAsync();
                        job?.Invoke();
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }

        public void Enqueue(Action job) => _writer.WriteAsync(job).GetAwaiter().GetResult();

        public void Stop() => _writer.Complete();
    }
}
