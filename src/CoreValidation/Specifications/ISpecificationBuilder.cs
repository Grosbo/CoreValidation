using System;
using System.Linq.Expressions;

namespace CoreValidation.Specifications
{
    /// <summary>
    /// Builder with all of the Fluent API extensions for specifying a valid state for <typeparamref name="TModel"/> models
    /// </summary>
    /// <typeparam name="TModel">Type to be specified.</typeparam>
    public interface ISpecificationBuilder<TModel>
        where TModel : class
    {
        /// <summary>
        /// Sets the specification for the selected member.
        /// </summary>
        /// <typeparam name="TMember">Type of the member.</typeparam>
        /// <param name="memberSelector">The member selector.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="memberSelector"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="memberSelector"/> is not pointing at property (but e.g. a field or method)</exception>
        /// <param name="memberSpecification">The member specification. If null, member has no rules, but it's required.</param>
        ISpecificationBuilder<TModel> Member<TMember>(Expression<Func<TModel, TMember>> memberSelector, MemberSpecification<TModel, TMember> memberSpecification = null);

        /// <summary>
        /// Sets the custom validation logic for the model in its global scope.
        /// </summary>
        /// <param name="isValid">Predicate determining if the current model is valid.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="isValid"/> is null</exception>
        ISpecificationBuilder<TModel> Valid(Predicate<TModel> isValid);


        /// <summary>
        /// Sets the error message that overrides all errors from the current scope.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null</exception>
        ISpecificationBuilder<TModel> SetSingleError(string message);


        /// <summary>
        /// Sets (and overrides) the error message of the preceding command. If placed after 'Member', replaces all errors from its scope.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null</exception>
        ISpecificationBuilder<TModel> WithMessage(string message);
    }
}