namespace DesignPatternsLibrary.RAII
{
    public struct WrapperStruct<T> : IMyType where T : class, IMyType, new()
    {
        private readonly T _value;

        public WrapperStruct()
        {
            _value = new();
        }

        public void Do() => _value.Do();
    }
}
