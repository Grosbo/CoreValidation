using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Commands
{
    internal abstract class AsModelRule : IRule
    {
        public Error RuleSingleError { get; set; }
        public string Name => "AsModel";
        public abstract bool TryGetErrors(object memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth, out IErrorsCollection errorsCollection);
    }

    internal sealed class AsModelRule<TMember> : AsModelRule
        where TMember : class
    {
        private IErrorsCollection _ruleSingleErrorInCollection;

        public AsModelRule(Specification<TMember> specification = null)
        {
            SpecificationId = specification?.GetHashCode().ToString();

            Specification = specification;
        }

        public string SpecificationId { get; }

        public Specification<TMember> Specification { get; }

        public override bool TryGetErrors(object memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth, out IErrorsCollection errorsCollection)
        {
            return TryGetErrors((TMember)memberValue, executionContext, validationStrategy, depth, out errorsCollection);
        }

        public bool TryGetErrors(TMember memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth, out IErrorsCollection errorsCollection)
        {
            var validator = executionContext.ValidatorsFactory.GetOrInit(Specification, SpecificationId);

            if ((validationStrategy == ValidationStrategy.Complete) && (RuleSingleError != null))
            {
                validationStrategy = ValidationStrategy.FailFast;
            }

            errorsCollection = ValidatorExecutor.Execute(validator, memberValue, executionContext, validationStrategy, depth + 1);

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