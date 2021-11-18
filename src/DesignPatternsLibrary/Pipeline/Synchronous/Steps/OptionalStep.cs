using System;

namespace DesignPatternsLibrary.Pipeline.Synchronous.Steps
{
    public class OptionalStep<TInput, TOutput> : IPipelineStep<TInput, TOutput> where TInput : TOutput
    {
        private IPipelineStep<TInput, TOutput> _step;
        private Func<TInput, bool> _choice;

        public OptionalStep(Func<TInput, bool> choice, IPipelineStep<TInput, TOutput> step)
        {
            _step = step;
            _choice = choice;
        }

        public TOutput Process(TInput input)
        {
            if (_choice(input))
            {
                return _step.Process(input);
            }
            else
            {
                return input;
            }
        }
    }
}
