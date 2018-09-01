using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Rules
{
    internal abstract class ValidNullableRule : IRule
    {
        protected static IReadOnlyCollection<Type> AllowedRulesTypes { get; } = new[]
        {
            typeof(ValidRule),
            typeof(ValidRelativeRule)
        };

        public abstract bool TryGetErrors(object model, object memberValue, IExecutionContext executionContext, out IErrorsCollection result);
    }

    internal sealed class ValidNullableRule<TModel, TMember> : ValidNullableRule
        where TModel : class
        where TMember : struct
    {
        public ValidNullableRule(MemberSpecification<TModel, TMember> memberSpecification)
        {
            MemberSpecification = memberSpecification ?? throw new ArgumentNullException(nameof(memberSpecification));

            MemberValidator = MemberValidatorCreator.Create(MemberSpecification);

            if (MemberValidator.Name != null)
            {
                throw new InvalidOperationException($"Cannot call {nameof(IMemberSpecificationBuilder<TModel, TMember>.WithName)} inside {nameof(ValidNullableRule)}");
            }

            var notAllowedRule = MemberValidator.Rules.FirstOrDefault(r =>
                AllowedRulesTypes.All(allowedType => !allowedType.IsInstanceOfType(r)));

            if (notAllowedRule != null)
            {
                throw new InvalidOperationException($"Invalid rule inside {nameof(ValidNullableRule)}");
            }
        }

        public IMemberValidator MemberValidator { get; }

        public MemberSpecification<TModel, TMember> MemberSpecification { get; }

        public override bool TryGetErrors(object model, object memberValue, IExecutionContext executionContext, out IErrorsCollection result)
        {
            return TryGetErrors((TModel)model, (TMember?)memberValue, executionContext, out result);
        }

        public bool TryGetErrors(TModel model, TMember? memberValue, IExecutionContext executionContext, out IErrorsCollection errorsCollection)
        {
            errorsCollection = ValidatorExecutor.ExecuteNullableMember(MemberValidator, model, memberValue, executionContext);

            return !errorsCollection.IsEmpty;
        }
    }
}