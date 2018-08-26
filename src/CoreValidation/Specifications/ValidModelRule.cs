using System;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal abstract class ValidModelRule
    {
        public abstract bool TryGetErrors(object memberValue, IRulesExecutionContext rulesExecutionContext, int depth, out IErrorsCollection errorsCollection);
    }

    internal sealed class ValidModelRule<TMember> : ValidModelRule, IRule
        where TMember : class
    {
        private readonly string _id;

        public ValidModelRule(Validator<TMember> validator = null)
        {
            _id = validator != null
                ? $"VMR_{Guid.NewGuid().ToString().Substring(0, 7).ToUpperInvariant()}"
                : null;

            Validator = validator;
        }

        public Validator<TMember> Validator { get; }

        public override bool TryGetErrors(object memberValue, IRulesExecutionContext rulesExecutionContext, int depth, out IErrorsCollection errorsCollection)
        {
            return TryGetErrors((TMember)memberValue, rulesExecutionContext, depth, out errorsCollection);
        }

        public bool TryGetErrors(TMember memberValue, IRulesExecutionContext rulesExecutionContext, int depth, out IErrorsCollection errorsCollection)
        {
            var specification = rulesExecutionContext.SpecificationsRepository.GetOrInit(Validator, _id);

            errorsCollection = SpecificationRulesExecutor.ExecuteSpecificationRules(specification, memberValue, rulesExecutionContext, depth + 1);

            return (errorsCollection != null) && !errorsCollection.IsEmpty;
        }
    }
}