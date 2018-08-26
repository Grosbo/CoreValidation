using System.Collections.Generic;
using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    internal interface ISpecification<TModel> where TModel :class
    {
        IReadOnlyCollection<IExecutableRule<TModel>> ExecutableRules { get; }
        Error SummaryError { get; }
    }
}