using CoreValidation.Errors;

namespace CoreValidation.Specifications.Commands
{
    internal interface ISingleErrorHolder
    {
        Error RuleSingleError { get; set; }
    }
}