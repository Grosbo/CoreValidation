using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoreValidation.Errors.Args;

namespace CoreValidation.Specifications
{
    public interface ISpecificationBuilder<TModel>
        where TModel : class
    {
        ISpecificationBuilder<TModel> For<TMember>(Expression<Func<TModel, TMember>> memberSelector, MemberSpecification<TModel, TMember> memberSpecification = null);

        ISpecificationBuilder<TModel> Valid(Predicate<TModel> isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null);

        ISpecificationBuilder<TModel> WithSummaryError(string message, IReadOnlyCollection<IMessageArg> args = null);
    }
}