using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsLibrary.MapReduce
{
    public static class MapReduceEx
    {
        public static IEnumerable<IGrouping<TKey, TMapped>> Map<TSource, TKey, TMapped>(this IList<TSource> source, Func<TSource, IEnumerable<TMapped>> map, Func<TMapped, TKey> keySelector)
        {  return source
                .AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .SelectMany(map)
                .GroupBy(keySelector)
                .ToList(); 
        }

        public static TResult[] Reduce<TKey, TMapped, TResult>(this IEnumerable<IGrouping<TKey, TMapped>> source, Func<IGrouping<TKey, TMapped>, TResult> reduce)
        {
            return source
                 .AsParallel()
                 .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                 .WithDegreeOfParallelism(Environment.ProcessorCount)
                 .Select(reduce).ToArray();
        }

        public static TResult[] MapReduce<TSource, TMapped, TKey, TResult>(
            this IList<TSource> source,
            Func<TSource, IEnumerable<TMapped>> map,
            Func<TMapped, TKey> keySelector,
            Func<IGrouping<TKey, TMapped>, TResult> reduce,
            int maxMapDegreeOfParallelism, int maxReduceDegreeOfParallelism)
        {
            return
                Partitioner.Create(source, true)
                .AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(maxMapDegreeOfParallelism)
                .SelectMany(map)
                .GroupBy(keySelector)
                .ToList().AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(maxReduceDegreeOfParallelism)
                .Select(reduce)
                .ToArray();
        }
    }
}
