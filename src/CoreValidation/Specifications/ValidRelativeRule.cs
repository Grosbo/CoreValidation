using System;
using System.Collections.Generic;
using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    public abstract class ValidRelativeRule
    {
    }

    public sealed class ValidRelativeRule<TModel> : ValidRelativeRule, IRule, IErrorMessageHolder
        where TModel : class
    {
        public ValidRelativeRule(Predicate<TModel> isValid, string message, IReadOnlyCollection<IMessageArg> args = null)
        {
            IsValid = isValid ?? throw new ArgumentNullException(nameof(isValid));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Arguments = args;
        }

        public Predicate<TModel> IsValid { get; }

        public IReadOnlyCollection<IMessageArg> Arguments { get; }

        public string Message { get; }

        public ErrorsCollection Compile(object[] args)
        {
            return Compile(
                IsValid,
                Message,
                Arguments,
                (TModel) args[0],
                (ValidationStrategy) args[1]
            );
        }

        public Error CompileError(TModel model, ValidationStrategy validationStrategy)
        {
            return CompileError(IsValid, Message, Arguments, model, validationStrategy);
        }

        public static ErrorsCollection Compile(Predicate<TModel> isValid, string message, IReadOnlyCollection<IMessageArg> args, TModel model, ValidationStrategy validationStrategy)
        {
            var errorsCollection = new ErrorsCollection();

            var error = CompileError(isValid, message, args, model, validationStrategy);

            if (error != null)
            {
                errorsCollection.AddError(error);
            }

            return errorsCollection;
        }

        public static Error CompileError(Predicate<TModel> isValid, string message, IReadOnlyCollection<IMessageArg> args, TModel model, ValidationStrategy validationStrategy)
        {
            if ((validationStrategy == ValidationStrategy.Force) ||
                ((model != null) && !isValid(model)))
            {
                return new Error(message, args);
            }

            return null;
        }
    }
}