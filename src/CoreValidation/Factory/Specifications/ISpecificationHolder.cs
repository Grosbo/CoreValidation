using CoreValidation.Specifications;

namespace CoreValidation.Factory.Specifications
{
    /// <summary>
    ///     Object that contains at least one specification.
    ///     This interface is only for detecting <see cref="ISpecificationHolder{T}" />, shouldn't be implemented directly.
    /// </summary>
    public interface ISpecificationHolder
    {
    }

    /// <summary>
    ///     Object that contains specification for type <typeparamref name="T"></typeparamref>.
    /// </summary>
    /// <typeparam name="T">Specified type.</typeparam>
    public interface ISpecificationHolder<T> : ISpecificationHolder
        where T : class
    {
        /// <summary>
        ///     Specification for type <typeparamref name="T" />.
        /// </summary>
        Specification<T> Specification { get; }
    }
}