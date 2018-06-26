using System.Collections.Generic;

namespace CoreValidation.Errors
{
    public interface IErrorsCollection
    {
        bool IsEmpty { get; }

        IReadOnlyCollection<Error> Errors { get; }

        IReadOnlyDictionary<string, IErrorsCollection> Members { get; }
    }
}