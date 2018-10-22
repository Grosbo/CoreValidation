using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Specifications.Commands;

namespace CoreValidation.Validators
{
    internal interface IMemberValidator
    {
        IReadOnlyCollection<IRule> Rules { get; }

        bool IsOptional { get; }

        IError SingleError { get; }

        IError RequiredError { get; }
    }
}