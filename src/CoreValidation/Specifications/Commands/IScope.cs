using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Commands
{
    internal interface IScope<in TModel> : IRule
        where TModel : class
    {
        bool TryGetErrors(TModel model, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth, out IErrorsCollection scopeErrorsCollection);

        void InsertErrors(ErrorsCollection targetErrorsCollection, IErrorsCollection scopeErrorsCollection);
    }
}