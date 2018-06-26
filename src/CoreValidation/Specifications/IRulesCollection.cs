using System.Collections.Generic;
using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    internal interface IRulesCollection
    {
        IReadOnlyCollection<IRule> Rules { get; }

        bool IsOptional { get; }

        Error SummaryError { get; }

        string Name { get; }

        Error RequiredError { get; }
    }
}