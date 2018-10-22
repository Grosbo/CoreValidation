using System;
using System.Linq.Expressions;

namespace CoreValidation.Specifications
{
    public interface ISpecificationBuilder<TModel>
        where TModel : class
    {
        ISpecificationBuilder<TModel> Member<TMember>(Expression<Func<TModel, TMember>> memberSelector, MemberSpecification<TModel, TMember> memberSpecification = null);

        ISpecificationBuilder<TModel> Valid(Predicate<TModel> isValid);

        ISpecificationBuilder<TModel> SetSingleError(string message);

        ISpecificationBuilder<TModel> WithMessage(string message);
    }
}