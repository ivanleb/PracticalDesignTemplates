using DesignPatternsLibrary.Observer.AsyncObserver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.ProcessRunner
{
    internal class DataReciever 
    {
        private readonly IAsyncObserver<string> _observer;

        public DataReciever(IAsyncObserver<string> observer)
        {
            _observer = observer;
        }

        public TaskCompletionSource<string[]> OutputResults { get; } = new TaskCompletionSource<string[]>();
        public List<string> Output { get; } = new List<string>();

        public void RecieveData(object sender, DataReceivedEventArgs args)
        {
            if (args.Data != null)
            {
                Output.Add(args.Data);
                _observer.OnNextAsync(args.Data);
            }
            else
                OutputResults.SetResult(Output.ToArray());
        }
    }
}
