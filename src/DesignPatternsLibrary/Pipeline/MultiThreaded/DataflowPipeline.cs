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
        private IJobProvider _jobProvider = new DefaultJobProvider();

        public DataflowPipeline(Func<TIn, DataflowPipeline<TIn, TOut>, TOut> steps)
        {
            steps?.Invoke(default(TIn), this);
            SetResultToLastStepBlock();
        }

        public DataflowPipeline<TIn, TOut> AddStep<TLocalIn, TLocalOut>(Func<TLocalIn, TLocalOut> stepFunc)
             => AddStep(stepFunc, maxDegreeOfParallelism: 1, maxCapacity: 1);

        public DataflowPipeline<TIn, TOut> AddStep<TLocalIn, TLocalOut>(Func<TLocalIn, TLocalOut> stepFunc, int maxDegreeOfParallelism, int maxCapacity)
        {
            TransformBlock<IJob<TLocalIn, TOut>, IJob<TLocalOut, TOut>> step = new(job =>
            {
                try
                {
                    return _jobProvider.CreateJob<TLocalOut, TOut>(stepFunc(job.Input), job.TaskCompletionSource);
                }
                catch (Exception e)
                {
                    job.TaskCompletionSource.SetException(e);
                    return _jobProvider.CreateJob<TLocalOut, TOut>(default(TLocalOut), job.TaskCompletionSource);
                }
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism, BoundedCapacity = maxCapacity });

            if (_steps.Count > 0)
            {
                IDataflowBlock lastStep = _steps.Last();
                ISourceBlock<IJob<TLocalIn, TOut>> targetBlock = lastStep.CastTo<ISourceBlock<IJob<TLocalIn, TOut>>>();

                targetBlock.LinkTo(
                    step, 
                    new DataflowLinkOptions (),
                    job => !job.TaskCompletionSource.Task.IsFaulted);

                targetBlock.LinkTo(
                    DataflowBlock.NullTarget<IJob<TLocalIn, TOut>>(), 
                    new DataflowLinkOptions(),
                    job => job.TaskCompletionSource.Task.IsFaulted);
            }
            _steps.Add(step);
            return this;
        }

        public DataflowPipeline<TIn, TOut> CreatePipeline(IJobProvider jobProvider = null)
        {
            SetResultToLastStepBlock();
            _jobProvider = jobProvider != null ?  jobProvider : _jobProvider;
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
            ITargetBlock<IJob<TIn, TOut>> firstStep = _steps[0].CastTo<ITargetBlock<IJob<TIn, TOut>>>();
            TaskCompletionSource<TOut> tcs = new();
            firstStep.SendAsync(_jobProvider.CreateJob<TIn, TOut>(input, tcs));
            return tcs.Task;
        }
    }
}
