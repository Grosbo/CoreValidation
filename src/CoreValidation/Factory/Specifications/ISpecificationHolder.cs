using CoreValidation.Specifications;

namespace CoreValidation.Factory.Specifications
{
    public interface ISpecificationHolder
    {
    }

    public interface ISpecificationHolder<T> : ISpecificationHolder
        where T : class
    {
        Specification<T> Specification { get; }
    }
}