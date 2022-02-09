using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    internal class GenericPipeline<TIn, TOut>
    {
        private readonly List<object> _pipelineSteps = new List<object>();

        public event Action<TOut> Finished;

        public GenericPipeline(Func<TIn, GenericPipeline<TIn, TOut>, TOut> steps)
        {
            steps.Invoke(default(TIn), this);
        }

        public void Execute(TIn input)
        {
            var first = _pipelineSteps[0] as IPipelineStep<TIn>;
            first.Buffer.Add(input);
        }

        public GenericPipelineStep<TStepIn, TStepOut> GenerateStep<TStepIn, TStepOut>()
        {
            var pipelineStep = new GenericPipelineStep<TStepIn, TStepOut>();
            var stepIndex = _pipelineSteps.Count;

            Task.Factory.StartNew(() =>
            {
                IPipelineStep<TStepOut> nextPipelineStep = null;

                foreach (var input in pipelineStep.Buffer.GetConsumingEnumerable())
                {
                    bool isLastStep = stepIndex == _pipelineSteps.Count - 1;
                    var output = pipelineStep.Action(input);
                    if (isLastStep)
                    {
                        Finished?.Invoke((TOut)(object)output);
                    }
                    else
                    {
                        nextPipelineStep = nextPipelineStep 
                            ?? (isLastStep 
                                        ? null 
                                        : _pipelineSteps[stepIndex + 1] as IPipelineStep<TStepOut>
                                );

                        nextPipelineStep?.Buffer.Add(output);
                    }
                }
            }, TaskCreationOptions.LongRunning);

            _pipelineSteps.Add(pipelineStep);
            return pipelineStep;
        }
    }
}
