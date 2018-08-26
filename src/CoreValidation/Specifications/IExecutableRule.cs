using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    internal interface IExecutableRule<in TModel> where TModel : class
    {
        bool TryExecuteAndGetErrors(TModel model, IRulesExecutionContext rulesExecutionContext, int depth, out IErrorsCollection errorsCollection);

        void InsertErrors(ErrorsCollection targetErrorsCollection, IErrorsCollection errorsCollectionToInclude);
    }
}