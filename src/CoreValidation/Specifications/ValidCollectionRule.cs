using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Options;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal abstract class ValidCollectionRule
    {
        protected static readonly RulesCollector RulesCollector = new RulesCollector();
    }

    internal sealed class ValidCollectionRule<TModel, TItem> : ValidCollectionRule, IRule
        where TModel : class
    {
        public ValidCollectionRule(MemberValidator<TModel, TItem> memberValidator)
        {
            MemberValidator = memberValidator ?? throw new ArgumentNullException(nameof(memberValidator));
        }

        public MemberValidator<TModel, TItem> MemberValidator { get; }

        public ErrorsCollection Compile(object[] args)
        {
            return Compile(
                MemberValidator,
                (TModel) args[0],
                args[1] as IEnumerable<TItem>,
                (IValidatorsRepository) args[2],
                (ValidationStrategy) args[3],
                (int) args[4],
                (IRulesOptions) args[5]
            );
        }

        public static ErrorsCollection Compile(MemberValidator<TModel, TItem> memberValidator, TModel model, IEnumerable<TItem> memberValue, IValidatorsRepository validatorsRepository, ValidationStrategy validationStrategy, int depth, IRulesOptions rulesOptions)
        {
            var errorsCollection = new ErrorsCollection();

            var memberSpecification = RulesCollector.GetMemberRules(memberValidator);

            if (memberSpecification.Name != null)
            {
                throw new InvalidOperationException($"Cannot call {nameof(IMemberSpecification<TModel, TItem>.WithName)} inside ${nameof(ValidCollectionRule)}");
            }

            var rulesCompiler = new RulesCompiler();

            if (validationStrategy == ValidationStrategy.Force)
            {
                var itemErrorsCollection = rulesCompiler.Compile(
                    memberSpecification,
                    model,
                    default(TItem),
                    validatorsRepository,
                    ValidationStrategy.Force,
                    depth,
                    rulesOptions
                );

                errorsCollection.AddError(rulesOptions.CollectionForceKey, itemErrorsCollection);
            }
            else if (memberValue != null)
            {
                var items = memberValue.ToArray();

                for (var i = 0; i < items.Length; i++)
                {
                    var item = items.ElementAt(i);

                    var itemErrorsCollection = rulesCompiler.Compile(
                        memberSpecification,
                        model,
                        item,
                        validatorsRepository,
                        validationStrategy,
                        depth,
                        rulesOptions
                    );

                    errorsCollection.AddError(i.ToString(), itemErrorsCollection);

                    if ((validationStrategy == ValidationStrategy.FailFast) && !errorsCollection.IsEmpty)
                    {
                        break;
                    }
                }
            }

            return errorsCollection;
        }
    }
}