using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Specifications.Commands;

namespace CoreValidation.Validators
{
    internal interface IValidator<in TModel> where TModel : class
    {
        IReadOnlyCollection<IScope<TModel>> Scopes { get; }
        IError SingleError { get; }
    }
}