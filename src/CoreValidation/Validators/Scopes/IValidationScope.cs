using CoreValidation.Errors;

namespace CoreValidation.Validators.Scopes
{
    internal interface IValidationScope<in TModel> where TModel : class
    {
        bool TryGetErrors(TModel model, IExecutionContext executionContext, int depth, out IErrorsCollection scopeErrorsCollection);

        void InsertErrors(ErrorsCollection targetErrorsCollection, IErrorsCollection scopeErrorsCollection);
    }
}