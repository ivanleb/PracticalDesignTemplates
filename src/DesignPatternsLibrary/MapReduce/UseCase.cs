using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsLibrary.MapReduce
{
    public static class UseCase
    {
        public static void Run()
        {
            Console.WriteLine("Start MapReduce use case");

            List<int> source = Enumerable.Range(0, 10000).ToList();
            var result = source.MapReduce(
                item => Enumerable.Range(0, item),
                item => item,
                group => group.Sum(),
                10,
                5);

            foreach (var item in result.OrderByDescending(item => item).Take(10)) 
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Finish MapReduce use case");
        }
    }
}
