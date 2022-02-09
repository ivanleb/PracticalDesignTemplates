using System;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    internal interface IPipeline
    {
        void Execute(object input);
        event Action<object> Finished;
    }
}
