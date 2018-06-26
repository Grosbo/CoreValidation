using System;
using System.Collections.Generic;
using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    public abstract class ValidRule
    {
    }

    public sealed class ValidRule<TMember> : ValidRule, IRule, IErrorMessageHolder
    {
        public ValidRule(Predicate<TMember> isValid, string message, IReadOnlyCollection<IMessageArg> args = null)
        {
            IsValid = isValid ?? throw new ArgumentNullException(nameof(isValid));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Arguments = args;
        }

        public Predicate<TMember> IsValid { get; }

        public IReadOnlyCollection<IMessageArg> Arguments { get; }

        public string Message { get; }

        public ErrorsCollection Compile(object[] args)
        {
            return Compile(
                IsValid,
                Message,
                Arguments,
                (TMember) args[0],
                (ValidationStrategy) args[1]
            );
        }

        public Error CompileError(TMember memberValue, ValidationStrategy validationStrategy)
        {
            return CompileError(IsValid, Message, Arguments, memberValue, validationStrategy);
        }

        public static ErrorsCollection Compile(Predicate<TMember> isValid, string message, IReadOnlyCollection<IMessageArg> args, TMember memberValue, ValidationStrategy validationStrategy)
        {
            var errorsCollection = new ErrorsCollection();

            var error = CompileError(isValid, message, args, memberValue, validationStrategy);

            if (error != null)
            {
                errorsCollection.AddError(error);
            }

            return errorsCollection;
        }

        public static Error CompileError(Predicate<TMember> isValid, string message, IReadOnlyCollection<IMessageArg> args, TMember memberValue, ValidationStrategy validationStrategy)
        {
            if ((validationStrategy == ValidationStrategy.Force) ||
                ((memberValue != null) && !isValid(memberValue)))
            {
                return new Error(message, args);
            }

            return null;
        }
    }
}