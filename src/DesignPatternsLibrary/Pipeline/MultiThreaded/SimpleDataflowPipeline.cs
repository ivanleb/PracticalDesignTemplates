using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    internal static class SimpleDataflowPipeline
    {
        public static TransformBlock<string, string> CreatePipeline(Action<bool> resultCallback)
        {
            var step1 = new TransformBlock<string, string>((sentence) => FindMostCommon(sentence));
            var step2 = new TransformBlock<string, int>((word) => word.Length);
            var step3 = new TransformBlock<int, bool>((length) => length % 2 == 1);
            var callbackStep = new ActionBlock<bool>(resultCallback);
            step1.LinkTo(step2, new DataflowLinkOptions());
            step2.LinkTo(step3, new DataflowLinkOptions());
            step3.LinkTo(callbackStep);
            return step1;
        }

        public static TransformBlock<string, string> CreatePipelineWithStepOptions(Action<bool> resultCallback)
        {
            var step1 = new TransformBlock<string, string>((sentence) => FindMostCommon(sentence),
                new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = 3,
                    BoundedCapacity = 5,
                });
            var step2 = new TransformBlock<string, int>((word) => word.Length,
                new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = 1,
                    BoundedCapacity = 13,
                });
            var step3 = new TransformBlock<int, bool>((length) => length % 2 == 1,
                new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = 11,
                    BoundedCapacity = 6,
                });
            var callBackStep = new ActionBlock<bool>(resultCallback);
            step1.LinkTo(step2, new DataflowLinkOptions());
            step2.LinkTo(step3, new DataflowLinkOptions());
            step3.LinkTo(callBackStep);
            return step1;
        }

        static string FindMostCommon(string input)
        {
            string[] splitted = input.Split(' ');
            Dictionary<string, int> counters = new Dictionary<string, int>();
            for (int i = 0; i < splitted.Length; i++)
            {
                if (counters.ContainsKey(splitted[i]))
                    counters[splitted[i]]++;
                else
                    counters[splitted[i]] = 1; ;
            }
            string mostCommon = counters.MaxBy(p => p.Value).Key;
            return mostCommon;
        }
    }
}
