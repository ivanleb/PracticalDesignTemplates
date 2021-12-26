namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public interface IContext<DataType>
    {
        DataType Data { get; }
    }
}
