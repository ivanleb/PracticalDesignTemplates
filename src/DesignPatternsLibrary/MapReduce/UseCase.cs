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
            ReduceToMultipleItems();
            ReduceToMultipleItems2();
            Console.WriteLine("Finish MapReduce use case");
        }

        private static void ReduceToMultipleItems()
        {

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
        }

        private static void ReduceToMultipleItems2() 
        {
            Person[] persons = Person.CreatePersons(100000);
            var results = persons
                   .Map(p => p.NickNames.Select(nn => new User { Id = Guid.NewGuid().ToString(), NickName = nn }),
                        user => user.NickName)
                   .Reduce(mapped => (mapped.Count(), mapped.Key)).OrderByDescending(item => item.Item1).Take(10);
            foreach (var item in results)
            {
                Console.WriteLine(item);
            }
        }
    }

    struct Person
    {
        public string Name { get; set; }
        public string[] NickNames { get; set; }

        public static Person[] CreatePersons(int n) 
        {
            Random random = new Random();
            List<Person> persons = new List<Person>();
            for (int i = 0; i < n; i++)
            {
                persons.Add(new Person { Name = $"Person {n}", NickNames = new string[] { $"NickName {random.Next(1, n)}", $"NickName {random.Next(n, 1000 + n)}" } });
            }

            return persons.ToArray();
        }
    }

    struct User 
    {
        public string Id { get; set; }
        public string NickName { get; set; }
    }
}
