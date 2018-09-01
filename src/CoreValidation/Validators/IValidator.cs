using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Validators.Scopes;

namespace CoreValidation.Validators
{
    internal interface IValidator<in TModel> where TModel : class
    {
        IReadOnlyCollection<IValidationScope<TModel>> Scopes { get; }
        Error SummaryError { get; }
    }
}