using DesignPatternsLibrary.Observer;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.ProcessRunner
{
    internal class ProcessCommunicator
    {
        private readonly Process _process;
        private readonly IAsyncObserver<string> _observer;
        private readonly DataReciever _standardOutputReciever;
        private readonly DataReciever _errorOutputReciever;

        public ProcessCommunicator(Process process, IAsyncObserver<string> observer)
        {
            ArgumentNullException.ThrowIfNull(process);
            ArgumentNullException.ThrowIfNull(observer);

            _process = process;
            _observer = observer;
            _standardOutputReciever = new DataReciever(observer);
            _errorOutputReciever = new DataReciever(observer);
        }

        public TaskCompletionSource<ProcessResults> Result { get; } = new TaskCompletionSource<ProcessResults>();
        public TaskCompletionSource<DateTime> ProcessStartTime { get; } = new TaskCompletionSource<DateTime>();

        public void RecieveStandartOutput(object sender, DataReceivedEventArgs args)
            => _standardOutputReciever.RecieveData(sender, args);        

        public void RecieveErrorOutput(object sender, DataReceivedEventArgs args)
            => _errorOutputReciever.RecieveData(sender, args);

        public async void OnExited(object? sender, EventArgs e)
        {
            Result.TrySetResult(
                new ProcessResults(
                    _process,
                    await ProcessStartTime.Task.ConfigureAwait(false),
                    await _standardOutputReciever.OutputResults.Task.ConfigureAwait(false),
                    await _errorOutputReciever.OutputResults.Task.ConfigureAwait(false)
                        )
                    );
            _observer.OnCompletedAsync();
        }

        public void OnCancel()
        {
            Result.TrySetCanceled();
            try
            {
                if (!_process.HasExited)
                    _process.Kill();
                _observer.OnNextAsync($"Process {_process.ProcessName} has been cancelled.");
            }
            catch (InvalidOperationException ex) 
            {
                _observer.OnErrorAsync(ex);
            }
        }
    }
}
