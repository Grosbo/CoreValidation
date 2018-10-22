using System;
using System.Reflection;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Commands
{
    internal sealed class MemberScope<TModel, TMember> : IScope<TModel> where TModel : class
    {
        private readonly Func<MemberScope<TModel, TMember>, TModel, IExecutionContext, ValidationStrategy, int, IErrorsCollection> _getErrors;

        public MemberScope(PropertyInfo memberPropertyInfo, MemberSpecification<TModel, TMember> memberSpecification)
        {
            MemberPropertyInfo = memberPropertyInfo ?? throw new ArgumentNullException(nameof(memberPropertyInfo));
            MemberValidator = MemberValidatorCreator.Create(memberSpecification ?? (m => m));
            Name = memberPropertyInfo.Name;

            _getErrors = (executableRule, model, executionContext, validationStrategy, depth) =>
            {
                var memberValue = model != null
                    ? (TMember)executableRule.MemberPropertyInfo.GetValue(model)
                    : default;

                return ValidatorExecutor.ExecuteMember(
                    executableRule.MemberValidator,
                    model,
                    memberValue,
                    executionContext,
                    validationStrategy,
                    depth
                );
            };
        }

        public IMemberValidator MemberValidator { get; }

        public PropertyInfo MemberPropertyInfo { get; }

        public string Name { get; }

        public bool TryGetErrors(TModel model, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth, out IErrorsCollection scopeErrorsCollection)
        {
            var errorsCollection = _getErrors(this, model, executionContext ?? throw new ArgumentNullException(nameof(executionContext)), validationStrategy, depth);

            if (!errorsCollection.IsEmpty && (RuleSingleError != null))
            {
                scopeErrorsCollection = ErrorsCollection.WithSingleOrNull(RuleSingleError);

                return true;
            }

            scopeErrorsCollection = errorsCollection;

            return !scopeErrorsCollection.IsEmpty;
        }

        public void InsertErrors(ErrorsCollection targetErrorsCollection, IErrorsCollection scopeErrorsCollection)
        {
            if (targetErrorsCollection == null)
            {
                throw new ArgumentNullException(nameof(targetErrorsCollection));
            }

            if (scopeErrorsCollection == null)
            {
                throw new ArgumentNullException(nameof(scopeErrorsCollection));
            }

            targetErrorsCollection.AddError(Name, scopeErrorsCollection);
        }

        public Error RuleSingleError { get; set; }
    }
}