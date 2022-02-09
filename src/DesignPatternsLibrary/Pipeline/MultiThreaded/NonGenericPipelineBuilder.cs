using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    internal class NonGenericPipelineBuilder : IPipeline
    {
        private readonly List<Func<object, object>> _pipelineSteps = new List<Func<object, object>>();
        private BlockingCollection<object>[] _buffers;

        public event Action<object> Finished;

        public void AddStep(Func<object, object> stepFunc)
        {
            _pipelineSteps.Add(stepFunc);
        }

        public void Execute(object input)
        {
            var first = _buffers[0];
            first.Add(input);
        }

        public IPipeline GetPipeline()
        {
            _buffers = _pipelineSteps 
                .Select(step => new BlockingCollection<object>())
                .ToArray();

            int bufferIndex = 0;
            foreach (Func<object, object> pipelineStep in _pipelineSteps)
            {
                int bufferIndexLocal = bufferIndex; 
                Task.Factory.StartNew(() =>
                {
                    foreach (var input in _buffers[bufferIndexLocal].GetConsumingEnumerable())
                    {
                        try
                        {
                            object output = pipelineStep.Invoke(input);

                            bool isLastStep = bufferIndexLocal == _pipelineSteps.Count - 1;
                            if (isLastStep)
                            {
                                Finished?.Invoke(output);
                            }
                            else
                            {
                                var next = _buffers[bufferIndexLocal + 1];
                                next.Add(output);
                            }
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                },TaskCreationOptions.LongRunning);
                bufferIndex++;
            }
            return this;
        }
    }
}
