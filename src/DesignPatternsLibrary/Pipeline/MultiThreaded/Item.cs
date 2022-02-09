using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    internal class Item<TIn, TOut> : IInputItem<TIn>, IOutputItem<TOut>
    {
        public Item(TIn input)
        {
            Input = input;
            TaskCompletionSource = new TaskCompletionSource<TOut>();
        }

        public TIn Input { get; }
        public TOut Output { get; set; }
        public TaskCompletionSource<TOut> TaskCompletionSource { get; }
    }

    internal interface IInputItem<TIn> 
    {
        TIn Input { get; }
    }

    internal interface IOutputItem<TOut> 
    {
        TOut Output { get; set; }
    }
}
