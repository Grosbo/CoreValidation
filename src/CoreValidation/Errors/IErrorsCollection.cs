using System.Collections.Generic;

namespace CoreValidation.Errors
{
    public interface IErrorsCollection
    {
        /// <summary>
        ///     Empty means no model scope errors (<see cref="Errors" />) and no member scope errors (<see cref="Members" />) are
        ///     in the collection.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        ///     Model scope errors.
        ///     They are indicating errors of the model as a whole.
        /// </summary>
        IReadOnlyCollection<IError> Errors { get; }

        /// <summary>
        ///     Member scope errors. They are indicating errors strictly related with the particular members.
        ///     Key is the member name.
        ///     Value is the <see cref="IErrorsCollection" /> with all errors related to that member.
        /// </summary>
        IReadOnlyDictionary<string, IErrorsCollection> Members { get; }
    }
}