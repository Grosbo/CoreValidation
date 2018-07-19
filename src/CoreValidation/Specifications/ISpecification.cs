using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    public interface ISpecification<TModel>
        where TModel : class
    {
        ISpecification<TModel> For<TMember>(Expression<Func<TModel, TMember>> memberSelector, MemberValidator<TModel, TMember> memberValidator = null);

        ISpecification<TModel> Valid(Predicate<TModel> isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null);

        ISpecification<TModel> WithSummaryError(string message, IReadOnlyCollection<IMessageArg> args = null);
    }
}