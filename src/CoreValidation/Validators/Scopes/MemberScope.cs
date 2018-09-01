using System;
using System.Reflection;
using CoreValidation.Errors;
using CoreValidation.Specifications;

namespace CoreValidation.Validators.Scopes
{
    internal sealed class MemberScope<TModel, TMember> : IValidationScope<TModel> where TModel : class
    {
        private readonly Func<MemberScope<TModel, TMember>, TModel, IExecutionContext, int, IErrorsCollection> _getErrors;

        public MemberScope(PropertyInfo memberPropertyInfo, MemberSpecification<TModel, TMember> memberSpecification)
        {
            MemberPropertyInfo = memberPropertyInfo;
            MemberValidator = MemberValidatorCreator.Create(memberSpecification ?? (m => m));
            Name = MemberValidator.Name ?? memberPropertyInfo.Name;

            _getErrors = (executableRule, model, rulesExecutionContext, depth) =>
            {
                var memberValue = model != null
                    ? (TMember)executableRule.MemberPropertyInfo.GetValue(model)
                    : default;

                return ValidatorExecutor.ExecuteMember(
                    executableRule.MemberValidator,
                    model,
                    memberValue,
                    rulesExecutionContext,
                    depth
                );
            };
        }

        public string Name { get; }

        public IMemberValidator MemberValidator { get; }

        public PropertyInfo MemberPropertyInfo { get; }

        public bool TryGetErrors(TModel model, IExecutionContext executionContext, int depth, out IErrorsCollection scopeErrorsCollection)
        {
            scopeErrorsCollection = _getErrors(this, model, executionContext, depth);

            return (scopeErrorsCollection != null) && !scopeErrorsCollection.IsEmpty;
        }

        public void InsertErrors(ErrorsCollection targetErrorsCollection, IErrorsCollection scopeErrorsCollection)
        {
            targetErrorsCollection.AddError(Name, scopeErrorsCollection);
        }
    }
}