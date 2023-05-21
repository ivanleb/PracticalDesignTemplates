using System;

namespace DesignPatternsLibrary.RAII
{
    public class MyClass : IMyType
    {    
        private readonly string _name;

        public MyClass()
        {
            _name = new Random().Next().ToString() + "_MyClass";
            Console.WriteLine(_name + "_created");
        }

        public void Do()
        {
            Console.WriteLine(_name + "_doing");
        }

        ~MyClass() 
        {
            Console.WriteLine(_name + "_destroyed");
        }
    }
}
