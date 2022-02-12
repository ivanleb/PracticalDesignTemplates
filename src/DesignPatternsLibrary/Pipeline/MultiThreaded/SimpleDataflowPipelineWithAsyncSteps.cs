using DesignPatternsLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    internal class SimpleDataflowPipelineWithAsyncSteps<TIn, TOut>
    {
        private List<(IDataflowBlock Block, bool IsAsync)> _steps = new List<(IDataflowBlock Block, bool IsAsync)>();

        public void AddStep<TLocalIn, TLocalOut>(Func<TLocalIn, TLocalOut> stepFunc)
        {
            if (_steps.Count == 0)
            {
                var step = new TransformBlock<TLocalIn, TLocalOut>(stepFunc);
                _steps.Add((step, IsAsync: false));
            }
            else
            {

                (var lastStep, bool islastStepAsync) = _steps.Last();
                if (!islastStepAsync)
                {
                    var step = new TransformBlock<TLocalIn, TLocalOut>(stepFunc);
                    var targetBlock = lastStep.CastTo<ISourceBlock<TLocalIn>>();
                    targetBlock.LinkTo(step, new DataflowLinkOptions());
                    _steps.Add((step, IsAsync: false));
                }
                else
                {
                    var step = new TransformBlock<Task<TLocalIn>, TLocalOut>(async (input) => stepFunc(await input));
                    var targetBlock = lastStep.CastTo<ISourceBlock<Task<TLocalIn>>>();
                    targetBlock.LinkTo(step, new DataflowLinkOptions());
                    _steps.Add((step, IsAsync: false));
                }
            }

        }

        public void AddStepAsync<TLocalIn, TLocalOut>
            (Func<TLocalIn, Task<TLocalOut>> stepFunc)
        {
            if (_steps.Count == 0)
            {
                var step = new TransformBlock<TLocalIn, Task<TLocalOut>>
                    (async (input) => await stepFunc(input));
                _steps.Add((step, IsAsync: true));
            }
            else
            {
                (var lastStep, bool islastStepAsync) = _steps.Last();
                if (islastStepAsync)
                {
                    var step = new TransformBlock<Task<TLocalIn>, Task<TLocalOut>>
                        (async (input) => await stepFunc(await input));
                    var targetBlock = lastStep.CastTo<ISourceBlock<Task<TLocalIn>>>();
                    targetBlock.LinkTo(step, new DataflowLinkOptions());
                    _steps.Add((step, IsAsync: true));
                }
                else
                {
                    var step = new TransformBlock<TLocalIn, Task<TLocalOut>>
                        (async (input) => await stepFunc(input));
                    var targetBlock = lastStep.CastTo<ISourceBlock<TLocalIn>>();
                    targetBlock.LinkTo(step, new DataflowLinkOptions());
                    _steps.Add((step, IsAsync: true));
                }
            }
        }

        public async Task CreatePipeline(Action<TOut> resultCallback)
        {
            (var lastStep, bool islastStepAsync) = _steps.Last();
            if (islastStepAsync)
            {
                var targetBlock = lastStep.CastTo<ISourceBlock<Task<TOut>>>();
                var callBackStep = new ActionBlock<Task<TOut>>
                    (async t => resultCallback(await t));
                targetBlock.LinkTo(callBackStep);
            }
            else
            {
                var callBackStep = new ActionBlock<TOut>(t => resultCallback(t));
                var targetBlock = lastStep.CastTo<ISourceBlock<TOut>>();
                targetBlock.LinkTo(callBackStep);
            }
        }

        public void Execute(TIn input)
        {
            var firstStep = _steps[0].Block as ITargetBlock<TIn>;
            firstStep.SendAsync(input);
        }
    }
}
