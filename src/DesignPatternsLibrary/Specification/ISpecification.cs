namespace DesignPatternsLibrary.Specification
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T item);
    }
}
