using CoreValidation.Results;
using CoreValidation.Translations;

namespace CoreValidation.Options
{
    /// <summary>
    ///     Options of the validation process.
    /// </summary>
    public interface IValidationOptions : IExecutionOptions
    {
        /// <summary>
        ///     Default translation name. Used to create <see cref="ITranslationProxy.DefaultTranslator" /> in the
        ///     <see cref="IValidationResult{T}" />.
        /// </summary>
        string TranslationName { get; }

        /// <summary>
        ///     Behavior for the null reference passed to be validated.
        /// </summary>
        NullRootStrategy NullRootStrategy { get; }

        /// <summary>
        ///     Strategy of the validation process.
        /// </summary>
        ValidationStrategy ValidationStrategy { get; }
    }
}