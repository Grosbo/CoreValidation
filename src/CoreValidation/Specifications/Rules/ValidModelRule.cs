using System;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Rules
{
    internal abstract class ValidModelRule
    {
        public abstract bool TryGetErrors(object memberValue, IExecutionContext executionContext, int depth, out IErrorsCollection errorsCollection);
    }

    internal sealed class ValidModelRule<TMember> : ValidModelRule, IRule
        where TMember : class
    {
        private readonly string _id;

        public ValidModelRule(Specification<TMember> specification = null)
        {
            _id = specification != null
                ? $"VMR_{Guid.NewGuid().ToString().Substring(0, 7).ToUpperInvariant()}"
                : null;

            Specification = specification;
        }

        public Specification<TMember> Specification { get; }

        public override bool TryGetErrors(object memberValue, IExecutionContext executionContext, int depth, out IErrorsCollection errorsCollection)
        {
            return TryGetErrors((TMember)memberValue, executionContext, depth, out errorsCollection);
        }

        public bool TryGetErrors(TMember memberValue, IExecutionContext executionContext, int depth, out IErrorsCollection errorsCollection)
        {
            var specification = executionContext.ValidatorsFactory.GetOrInit(Specification, _id);

            errorsCollection = ValidatorExecutor.Execute(specification, memberValue, executionContext, depth + 1);

            return (errorsCollection != null) && !errorsCollection.IsEmpty;
        }
    }
}