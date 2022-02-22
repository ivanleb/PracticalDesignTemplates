using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsLibrary.MapReduce
{
    public static class MapReduceEx
    {
        public static TResult[] MapReduce<TSource, TMapped, TKey, TResult>(
            this IList<TSource> source,
            Func<TSource, IEnumerable<TMapped>> map,
            Func<TMapped, TKey> keySelector,
            Func<IGrouping<TKey, TMapped>, TResult> reduce,
            int maxMapDegreeOfParallelism, int maxReduceDegreeOfParallelism)
        {
            OrderablePartitioner<TSource> partitioner = Partitioner.Create(source, true);

            TResult[] mapResults =
                partitioner.AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(maxMapDegreeOfParallelism)
                .SelectMany(map)
                .GroupBy(keySelector)
                .ToList().AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(maxReduceDegreeOfParallelism)
                .Select(reduce)
                .ToArray();

            return mapResults;
        }
    }
}
