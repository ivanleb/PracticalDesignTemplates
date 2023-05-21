using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.RAII
{
    public static class UseCase
    {
        public static void Run() 
        {
            Console.WriteLine("Raii use case");
            for (int i = 0; i < 100; i++)
            {
                RunIteration();
            }

            for (int i = 0; i < 100; i++)
            {
                Task.Delay(100).Wait();
                Console.WriteLine("[DesignPatternsLibrary.RAII] Cycle after use case");
            }
        }

        static void RunIteration() 
        {
            WrapperStruct<MyClass> raiiObject = new WrapperStruct<MyClass>();
            raiiObject.Do();
        }
    }
}
