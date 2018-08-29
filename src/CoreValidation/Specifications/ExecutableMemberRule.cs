using System;
using System.Reflection;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal sealed class ExecutableMemberRule<TModel, TMember> : IExecutableRule<TModel> where TModel : class
    {
        private readonly Func<ExecutableMemberRule<TModel, TMember>, TModel, IRulesExecutionContext, int, IErrorsCollection> _getErrors;

        public ExecutableMemberRule(PropertyInfo memberPropertyInfo, MemberValidator<TModel, TMember> memberValidator)
        {
            MemberPropertyInfo = memberPropertyInfo;
            MemberSpecification = MemberValidatorProcessor.Process(memberValidator ?? (m => m));
            Name = MemberSpecification.Name ?? memberPropertyInfo.Name;

            _getErrors = (executableRule, model, rulesExecutionContext, depth) =>
            {
                var memberValue = model != null
                    ? (TMember)executableRule.MemberPropertyInfo.GetValue(model)
                    : default;

                return SpecificationRulesExecutor.ExecuteMemberSpecificationRules(
                    executableRule.MemberSpecification,
                    model,
                    memberValue,
                    rulesExecutionContext,
                    depth
                );
            };
        }

        public string Name { get; }

        public IMemberSpecification MemberSpecification { get; }

        public PropertyInfo MemberPropertyInfo { get; }

        public bool TryExecuteAndGetErrors(TModel model, IRulesExecutionContext rulesExecutionContext, int depth, out IErrorsCollection errorsCollection)
        {
            errorsCollection = _getErrors(this, model, rulesExecutionContext, depth);

            return (errorsCollection != null) && !errorsCollection.IsEmpty;
        }

        public void InsertErrors(ErrorsCollection targetErrorsCollection, IErrorsCollection errorsCollectionToInclude)
        {
            targetErrorsCollection.AddError(Name, errorsCollectionToInclude);
        }
    }
}