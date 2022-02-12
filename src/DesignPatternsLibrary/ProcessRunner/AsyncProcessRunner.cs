using DesignPatternsLibrary.Observer;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.ProcessRunner
{
    public static class AsyncProcessRunner
    {
        public static async Task<ProcessResults> Run(ProcessStartInfo processStartInfo, IAsyncObserver<string> observer, CancellationToken cancellationToken)
        {
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            var process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            ProcessCommunicator processCommunicator = new ProcessCommunicator(process, observer);

            process.OutputDataReceived += processCommunicator.RecieveStandartOutput;
            process.ErrorDataReceived += processCommunicator.RecieveErrorOutput;
            process.Exited += processCommunicator.OnExited;

            using (cancellationToken.Register(processCommunicator.OnCancel))
            {
                cancellationToken.ThrowIfCancellationRequested();

                DateTime startTime = DateTime.Now;

                try
                {
                    if (process.Start())
                    {
                        startTime = process.StartTime;
                        processCommunicator.ProcessStartTime.SetResult(startTime);

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                    }
                    else
                    {
                        var ex = new InvalidOperationException("Failed to start process");
                        processCommunicator.Result.TrySetException(ex);
                        observer.OnErrorAsync(ex);
                    }
                }
                catch (Exception ex)
                {
                    processCommunicator.Result.TrySetException(ex);
                    observer.OnErrorAsync(ex);
                }

                return await processCommunicator.Result.Task.ConfigureAwait(false);
            }
        }

        public static Task<ProcessResults> Run(string fileName)
            => Run(new ProcessStartInfo(fileName));

        public static Task<ProcessResults> Run(string fileName, CancellationToken cancellationToken)
            => Run(new ProcessStartInfo(fileName), cancellationToken);

        public static Task<ProcessResults> Run(string fileName, string arguments)
            => Run(new ProcessStartInfo(fileName, arguments));

        public static Task<ProcessResults> Run(string fileName, string arguments, CancellationToken cancellationToken)
            => Run(new ProcessStartInfo(fileName, arguments), cancellationToken);

        public static Task<ProcessResults> Run(ProcessStartInfo processStartInfo)
            => Run(processStartInfo, CancellationToken.None);

        public static Task<ProcessResults> Run(ProcessStartInfo processStartInfo, CancellationToken cancellationToken)
            => Run(processStartInfo, new AsyncObserverEmpty<string>(), cancellationToken);

        public static Task<ProcessResults> Run(string fileName, IAsyncObserver<string> observer)
            => Run(new ProcessStartInfo(fileName), observer);

        public static Task<ProcessResults> Run(string fileName, IAsyncObserver<string> observer, CancellationToken cancellationToken)
            => Run(new ProcessStartInfo(fileName), observer, cancellationToken);

        public static Task<ProcessResults> Run(string fileName, string arguments, IAsyncObserver<string> observer)
            => Run(new ProcessStartInfo(fileName, arguments), observer);

        public static Task<ProcessResults> Run(string fileName, string arguments, IAsyncObserver<string> observer, CancellationToken cancellationToken)
            => Run(new ProcessStartInfo(fileName, arguments), observer, cancellationToken);

        public static Task<ProcessResults> Run(ProcessStartInfo processStartInfo, IAsyncObserver<string> observer)
            => Run(processStartInfo, observer, CancellationToken.None);
    }
}
