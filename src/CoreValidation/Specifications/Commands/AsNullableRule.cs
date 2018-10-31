using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Commands
{
    internal abstract class AsNullableRule : IRule
    {
        protected static IReadOnlyCollection<Type> AllowedCommands { get; } = new[]
        {
            typeof(ValidRule),
            typeof(AsRelativeRule),
            typeof(SetSingleErrorCommand),
            typeof(WithMessageCommand)
        };

        public Error RuleSingleError { get; set; }
        public string Name => "AsNullable";

        public abstract bool TryGetErrors(object model, object memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, out IErrorsCollection result);
    }

    internal sealed class AsNullableRule<TModel, TMember> : AsNullableRule
        where TModel : class
        where TMember : struct
    {
        private IErrorsCollection _ruleSingleErrorInCollection;

        public AsNullableRule(MemberSpecification<TModel, TMember> memberSpecification)
        {
            MemberSpecification = memberSpecification ?? throw new ArgumentNullException(nameof(memberSpecification));

            MemberValidator = MemberValidatorCreator.Create(MemberSpecification, out var commands);

            var invalidCommand = commands.FirstOrDefault(r =>
                AllowedCommands.All(allowedType => !allowedType.IsInstanceOfType(r)));

            if (invalidCommand != null)
            {
                throw new InvalidCommandException($"Command {invalidCommand.Name} is not allowed within {Name}");
            }
        }

        public IMemberValidator MemberValidator { get; }

        public MemberSpecification<TModel, TMember> MemberSpecification { get; }

        public override bool TryGetErrors(object model, object memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, out IErrorsCollection result)
        {
            return TryGetErrors((TModel)model, (TMember?)memberValue, executionContext, validationStrategy, out result);
        }

        public bool TryGetErrors(TModel model, TMember? memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, out IErrorsCollection errorsCollection)
        {
            errorsCollection = ValidatorExecutor.ExecuteNullableMember(MemberValidator, model, memberValue, executionContext, validationStrategy);

            if (!errorsCollection.IsEmpty && (RuleSingleError != null))
            {
                if (errorsCollection.ContainsSingleError())
                {
                    errorsCollection = ErrorsCollection.WithSingleOrNull(new Error(RuleSingleError.Message, errorsCollection.GetSingleError().Arguments));
                }
                else
                {
                    if (_ruleSingleErrorInCollection == null)
                    {
                        _ruleSingleErrorInCollection = ErrorsCollection.WithSingleOrNull(RuleSingleError);
                    }

                    errorsCollection = _ruleSingleErrorInCollection;
                }

                return true;
            }

            return !errorsCollection.IsEmpty;
        }
    }
}