using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class ChannelAsyncPipeline
    {
        private readonly ChannelWriter<Action> _writer;

        public ChannelAsyncPipeline()
        {
            Channel<Action> channel = Channel.CreateUnbounded<Action>();
            ChannelReader<Action> reader = channel.Reader;
            _writer = channel.Writer;

            Task.Factory.StartNew(async () =>
            {
                while (await reader.WaitToReadAsync())
                {
                    Action job = await reader.ReadAsync();
                    job?.Invoke();
                }
            }, TaskCreationOptions.LongRunning);
        }

        public async Task Enqueue(Action job) => await _writer.WriteAsync(job);
       
        public void Stop() => _writer.Complete();
    }
}
