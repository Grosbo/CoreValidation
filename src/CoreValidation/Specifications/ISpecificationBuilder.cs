using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    public interface ISpecificationBuilder<TModel>
        where TModel : class
    {
        ISpecificationBuilder<TModel> For<TMember>(Expression<Func<TModel, TMember>> memberSelector, MemberValidator<TModel, TMember> memberValidator = null);

        ISpecificationBuilder<TModel> Valid(Predicate<TModel> isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null);

        ISpecificationBuilder<TModel> WithSummaryError(string message, IReadOnlyCollection<IMessageArg> args = null);
    }
}