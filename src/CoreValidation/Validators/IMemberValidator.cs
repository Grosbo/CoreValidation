using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Specifications.Rules;

namespace CoreValidation.Validators
{
    internal interface IMemberValidator
    {
        IReadOnlyCollection<IRule> Rules { get; }

        bool IsOptional { get; }

        Error SummaryError { get; }

        string Name { get; }

        Error RequiredError { get; }
    }
}