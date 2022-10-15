using System;

namespace DesignPatternsLibrary.Disposable
{
    public static class UseCase
    {
        public static void Run()
        {
            Console.WriteLine("AutoDisposeStack use case");
            using AutoDisposeStack autoDisposeStack = new AutoDisposeStack(allowNullObjects: true, throwItemDisposeException: true);
            for (int i = 0; i < 10; i++)
            {
                DisposableObject disposableObject = new DisposableObject($"object {i}");
                autoDisposeStack.Add(disposableObject);
            }
        }
    }

    public class DisposableObject : IDisposable
    {
        private readonly string _objectName;

        public DisposableObject(string objectName)
        {
            _objectName = objectName;
        }

        public void Dispose()
        {
            Console.WriteLine($"Disposing {_objectName}");
        }
    }
}
