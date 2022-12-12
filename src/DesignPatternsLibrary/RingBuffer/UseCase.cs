using System;

namespace DesignPatternsLibrary.RingBuffer
{
    public static class UseCase
    {
        public static void Run() 
        {
            Console.WriteLine("RingBuffer use case");
            RingBuffer<string> buffer = new RingBuffer<string>(10);
            bool isFull = true;
            for (int i = 0; i < 100; i++)
            {
                if (isFull)
                    isFull = buffer.Enqueue($"str{i}");
                else
                { 
                    isFull = !buffer.Dequeue(out string str);
                    Console.WriteLine($"{i} : {str}");
                }
            }
        }
    }
}
