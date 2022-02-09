using DesignPatternsLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    public class DataflowPipeline<TIn, TOut>
    {
        private List<IDataflowBlock> _steps = new List<IDataflowBlock>();

        public DataflowPipeline(Func<TIn, DataflowPipeline<TIn, TOut>, TOut> steps)
        {
            steps?.Invoke(default(TIn), this);
            SetResultToLastStepBlock();
        }

        public DataflowPipeline<TIn, TOut> AddStep<TLocalIn, TLocalOut>(Func<TLocalIn, TLocalOut> stepFunc)
             => AddStep(stepFunc, maxDegreeOfParallelism: 1, maxCapacity: 1);

        public DataflowPipeline<TIn, TOut> AddStep<TLocalIn, TLocalOut>(Func<TLocalIn, TLocalOut> stepFunc, int maxDegreeOfParallelism, int maxCapacity)
        {
            TransformBlock<Job<TLocalIn, TOut>, Job<TLocalOut, TOut>> step = new(job =>
            {
                try
                {
                    return new Job<TLocalOut, TOut>(stepFunc(job.Input), job.TaskCompletionSource);
                }
                catch (Exception e)
                {
                    job.TaskCompletionSource.SetException(e);
                    return new Job<TLocalOut, TOut>(default(TLocalOut), job.TaskCompletionSource);
                }
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism, BoundedCapacity = maxCapacity });

            if (_steps.Count > 0)
            {
                IDataflowBlock lastStep = _steps.Last();
                ISourceBlock<Job<TLocalIn, TOut>> targetBlock = lastStep.CastTo<ISourceBlock<Job<TLocalIn, TOut>>>();

                targetBlock.LinkTo(
                    step, 
                    new DataflowLinkOptions (),
                    job => !job.TaskCompletionSource.Task.IsFaulted);

                targetBlock.LinkTo(
                    DataflowBlock.NullTarget<Job<TLocalIn, TOut>>(), 
                    new DataflowLinkOptions(),
                    job => job.TaskCompletionSource.Task.IsFaulted);
            }
            _steps.Add(step);
            return this;
        }

        public DataflowPipeline<TIn, TOut> CreatePipeline()
        {
            SetResultToLastStepBlock();
            return this;
        }

        private void SetResultToLastStepBlock()
        {
            ActionBlock<Job<TOut, TOut>> setResultStep = new(job => job.TaskCompletionSource.SetResult(job.Input));
            IDataflowBlock lastStep = _steps.Last();
            ISourceBlock<Job<TOut, TOut>> setResultBlock = lastStep.CastTo<ISourceBlock<Job<TOut, TOut>>>();
            setResultBlock.LinkTo(setResultStep);
        }

        public Task<TOut> Execute(TIn input)
        {
            ITargetBlock<Job<TIn, TOut>> firstStep = _steps[0].CastTo<ITargetBlock<Job<TIn, TOut>>>();
            TaskCompletionSource<TOut> tcs = new();
            firstStep.SendAsync(new Job<TIn, TOut>(input, tcs));
            return tcs.Task;
        }
    }
}
