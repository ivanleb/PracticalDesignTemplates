using System;

namespace DesignPatternsLibrary.NTimesValue
{
    public static class UseCase
    {
        public static void Run()
        {
            Console.WriteLine("NTimesValue use case");
            NTimesValue<String> nTimesValue = new NTimesValue<String>($"Value: String {DateTime.Now}", maxAccessCount: 2);
            Console.WriteLine("First access");
            if (nTimesValue.TryGetValue(out string value1))
            {
                Console.WriteLine(value1);
            }
            else
            {
                Console.WriteLine("Value has been accessed the maximum number of times.");
            }
            Console.WriteLine("Second access");
            if (nTimesValue.TryGetValue(out string value2))
            {
                Console.WriteLine(value2);
            }
            else
            {
                Console.WriteLine("Value has been accessed the maximum number of times.");
            }
            Console.WriteLine("Third access");
            if (nTimesValue.TryGetValue(out string value3))
            {
                Console.WriteLine(value3);
            }
            else
            {
                Console.WriteLine("Value has been accessed the maximum number of times.");
            }

        }
    }
}
