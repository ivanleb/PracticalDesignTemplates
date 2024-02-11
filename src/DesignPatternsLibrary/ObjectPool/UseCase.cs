using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.ObjectPool
{
    public static class UseCase
    {
        public static void Run()
        {
            Console.WriteLine("Start ObjectPool use case");
            var pool = new ObjectPool<ExpensiveResource>();
            var resource = pool.GetObject();
            resource.Value = 42;
            pool.ReturnObject(resource);
            Console.WriteLine("Finish ObjectPool use case");
        }
    }
}
