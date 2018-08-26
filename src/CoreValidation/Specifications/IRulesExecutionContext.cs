using CoreValidation.Errors;
using CoreValidation.Options;

namespace CoreValidation.Specifications
{
    internal interface IRulesExecutionContext
    {
        IRulesOptions RulesOptions { get; }
        ISpecificationsRepository SpecificationsRepository { get; }
        ValidationStrategy ValidationStrategy { get; }
        ErrorsCollection DefaultErrorCollection { get; }
    }
}