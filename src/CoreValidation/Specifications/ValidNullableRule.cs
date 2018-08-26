using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal abstract class ValidNullableRule: IRule
    {
        public abstract bool TryGetErrors(object model, object memberValue, IRulesExecutionContext rulesExecutionContext, out IErrorsCollection result);

        protected static IReadOnlyCollection<Type> AllowedRulesTypes { get; } = new[]
        {
            typeof(ValidRule),
            typeof(ValidRelativeRule)
        };
    }

    internal sealed class ValidNullableRule<TModel, TMember> : ValidNullableRule
        where TModel : class
        where TMember : struct
    {
        public ValidNullableRule(MemberValidator<TModel, TMember> memberValidator)
        {
            MemberValidator = memberValidator ?? throw new ArgumentNullException(nameof(memberValidator));

            MemberSpecification = MemberValidatorProcessor.Process(MemberValidator);

            if (MemberSpecification.Name != null)
            {
                throw new InvalidOperationException($"Cannot call {nameof(IMemberSpecificationBuilder<TModel, TMember>.WithName)} inside {nameof(ValidNullableRule)}");
            }

            var notAllowedRule = MemberSpecification.Rules.FirstOrDefault(r =>
                AllowedRulesTypes.All(allowedType => !allowedType.IsInstanceOfType(r)));

            if (notAllowedRule != null)
            {
                throw new InvalidOperationException($"Invalid rule inside {nameof(ValidNullableRule)}");
            }
        }

        public IMemberSpecification MemberSpecification { get; }

        public MemberValidator<TModel, TMember> MemberValidator { get; }

        public override bool TryGetErrors(object model, object memberValue, IRulesExecutionContext rulesExecutionContext, out IErrorsCollection result)
        {
            return TryGetErrors((TModel)model, (TMember?)memberValue, rulesExecutionContext, out result);
        }

        public bool TryGetErrors(TModel model, TMember? memberValue, IRulesExecutionContext rulesExecutionContext, out IErrorsCollection errorsCollection)
        {
            errorsCollection = SpecificationRulesExecutor.ExecuteNullableMemberSpecificationRules(MemberSpecification, model, memberValue, rulesExecutionContext);

            return !errorsCollection.IsEmpty;
        }
    }
}