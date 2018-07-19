using System;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal abstract class ValidNullableRule
    {
        protected static readonly RulesCollector RulesCollector = new RulesCollector();
    }

    internal sealed class ValidNullableRule<TModel, TMember> : ValidNullableRule, IRule
        where TModel : class
        where TMember : struct
    {
        public ValidNullableRule(MemberValidator<TModel, TMember> memberValidator)
        {
            MemberValidator = memberValidator ?? throw new ArgumentNullException(nameof(memberValidator));
        }

        public MemberValidator<TModel, TMember> MemberValidator { get; }

        public ErrorsCollection Compile(object[] args)
        {
            return Compile(
                MemberValidator,
                (TModel) args[0],
                (TMember?) args[1],
                (ValidationStrategy) args[2],
                (Error)args[3]
            );
        }


        public static ErrorsCollection Compile(MemberValidator<TModel, TMember> memberValidator, TModel model, TMember? memberValue, ValidationStrategy validationStrategy, Error defaultError)
        {
            var errorsCollection = new ErrorsCollection();

            var rulesCollection = RulesCollector.GetMemberRules(memberValidator);

            if ((validationStrategy == ValidationStrategy.Complete) && (rulesCollection.SummaryError != null))
            {
                validationStrategy = ValidationStrategy.FailFast;
            }

            if (rulesCollection.Name != null)
            {
                throw new InvalidOperationException($"Cannot call {nameof(IMemberSpecification<TModel, TMember>.WithName)} inside {nameof(ValidNullableRule)}");
            }

            foreach (var innerRule in rulesCollection.Rules)
            {
                Error error = null;

                if (innerRule is ValidRule<TMember> validateRule)
                {
                    if ((validationStrategy == ValidationStrategy.Force) ||
                        (memberValue.HasValue && !validateRule.IsValid(memberValue.Value)))
                    {
                        error = Error.CreateOrDefault(validateRule.Message, validateRule.Arguments, defaultError);
                    }
                }
                else if (innerRule is ValidRelativeRule<TModel> relationRule)
                {
                    if ((validationStrategy == ValidationStrategy.Force) ||
                        (memberValue.HasValue && !relationRule.IsValid(model)))
                    {
                        error = Error.CreateOrDefault(relationRule.Message, relationRule.Arguments, defaultError);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Unknown rule in {nameof(ValidNullableRule)}");
                }

                if (error != null)
                {
                    if (rulesCollection.SummaryError != null)
                    {
                        errorsCollection.AddError(rulesCollection.SummaryError);

                        break;
                    }

                    errorsCollection.AddError(error);

                    if (validationStrategy == ValidationStrategy.FailFast)
                    {
                        break;
                    }
                }
            }

            return errorsCollection;
        }
    }
}