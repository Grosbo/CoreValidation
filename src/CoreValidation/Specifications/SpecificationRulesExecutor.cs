using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;

namespace CoreValidation.Specifications
{
    internal static class SpecificationRulesExecutor
    {
        public static IErrorsCollection ExecuteSpecificationRules<TModel>(ISpecification<TModel> specification, TModel model, IRulesExecutionContext rulesExecutionContext, int depth)
            where TModel : class
        {
            if (depth > rulesExecutionContext.RulesOptions.MaxDepth)
            {
                throw new MaxDepthExceededException(rulesExecutionContext.RulesOptions.MaxDepth);
            }

            ErrorsCollection errorsCollection = null;

            var validationStrategy = rulesExecutionContext.ValidationStrategy;

            if ((validationStrategy == ValidationStrategy.Complete) && (specification.SummaryError != null))
            {
                validationStrategy = ValidationStrategy.FailFast;
            }

            foreach (var compilableRule in specification.ExecutableRules)
            {
                if ((errorsCollection != null) && !errorsCollection.IsEmpty && (validationStrategy == ValidationStrategy.FailFast))
                {
                    break;
                }

                if (compilableRule.TryExecuteAndGetErrors(model, rulesExecutionContext, depth, out var ruleErrorsCollection))
                {
                    if (errorsCollection == null)
                    {
                        errorsCollection = new ErrorsCollection();
                    }

                    compilableRule.InsertErrors(errorsCollection, ruleErrorsCollection);
                }
            }

            if ((errorsCollection != null) && !errorsCollection.IsEmpty && (specification.SummaryError != null))
            {
                var summaryErrorCollection = new ErrorsCollection();
                summaryErrorCollection.AddError(specification.SummaryError);

                return summaryErrorCollection;
            }

            return errorsCollection ?? ErrorsCollection.Empty;
        }

        public static IErrorsCollection ExecuteMemberSpecificationRules<TModel, TMember>(IMemberSpecification memberSpecification, TModel model, TMember memberValue, IRulesExecutionContext rulesExecutionContext, int depth)
            where TModel : class
        {
            ErrorsCollection result = null;

            // ReSharper disable once CompareNonConstrainedGenericWithNull
            var exists = IsValueType(typeof(TMember)) || (memberValue != null);

            var validationStrategy = rulesExecutionContext.ValidationStrategy;

            var includeRequired = !memberSpecification.IsOptional && (!exists || (validationStrategy == ValidationStrategy.Force));

            if (includeRequired)
            {
                result = new ErrorsCollection();
                result.AddError(memberSpecification.RequiredError ?? rulesExecutionContext.RulesOptions.RequiredError);
            }

            if ((validationStrategy == ValidationStrategy.Complete) && (memberSpecification.SummaryError != null))
            {
                validationStrategy = ValidationStrategy.FailFast;
            }

            if ((validationStrategy == ValidationStrategy.FailFast) && (result != null) && !result.IsEmpty)
            {
                return result;
            }

            if (exists || (validationStrategy == ValidationStrategy.Force))
            {
                foreach (var rule in memberSpecification.Rules)
                {
                    bool anyErrors;

                    IErrorsCollection ruleErrorsCollection;

                    if (rule is ValidRule<TMember> validateRule)
                    {
                        anyErrors = validateRule.TryGetErrors(memberValue, rulesExecutionContext, out ruleErrorsCollection);
                    }
                    else if (rule is ValidRelativeRule<TModel> validateRelationRule)
                    {
                        anyErrors = validateRelationRule.TryGetErrors(model, rulesExecutionContext, out ruleErrorsCollection);
                    }
                    else if (rule is ValidModelRule validModelRule)
                    {
                        anyErrors = validModelRule.TryGetErrors(memberValue, rulesExecutionContext, depth, out ruleErrorsCollection);
                    }
                    else if (rule is ValidCollectionRule validCollectionRule)
                    {
                        anyErrors = validCollectionRule.TryGetErrors(model, memberValue, rulesExecutionContext, depth, out ruleErrorsCollection);
                    }
                    else if (rule is ValidNullableRule validNullableRule)
                    {
                        anyErrors = validNullableRule.TryGetErrors(model, memberValue, rulesExecutionContext, out ruleErrorsCollection);
                    }
                    else
                    {
                        throw new InvalidOperationException("Unknown rule");
                    }

                    if (!anyErrors)
                    {
                        continue;
                    }

                    if (result == null)
                    {
                        result = new ErrorsCollection();
                    }

                    result.Include(ruleErrorsCollection);

                    if (validationStrategy == ValidationStrategy.FailFast)
                    {
                        break;
                    }
                }
            }

            if ((result == null) || result.IsEmpty)
            {
                return ErrorsCollection.Empty;
            }

            if (memberSpecification.SummaryError == null)
            {
                return result;
            }


            var summaryErrorResult = new ErrorsCollection();

            if (includeRequired)
            {
                summaryErrorResult.AddError(rulesExecutionContext.RulesOptions.RequiredError ?? memberSpecification.RequiredError);
            }

            summaryErrorResult.AddError(memberSpecification.SummaryError);

            return summaryErrorResult;
        }

        public static IErrorsCollection ExecuteNullableMemberSpecificationRules<TModel, TMember>(IMemberSpecification memberSpecification, TModel model, TMember? memberValue, IRulesExecutionContext rulesExecutionContext)
            where TModel : class
            where TMember : struct
        {
            ErrorsCollection result = null;

            var validationStrategy = rulesExecutionContext.ValidationStrategy;

            if ((validationStrategy == ValidationStrategy.Complete) && (memberSpecification.SummaryError != null))
            {
                validationStrategy = ValidationStrategy.FailFast;
            }

            foreach (var rule in memberSpecification.Rules)
            {
                Error error = null;

                if (rule is ValidRule<TMember> validateRule)
                {
                    if ((validationStrategy == ValidationStrategy.Force) ||
                        (memberValue.HasValue && !validateRule.IsValid(memberValue.Value)))
                    {
                        error = validateRule.Error ?? rulesExecutionContext.RulesOptions.DefaultError;
                    }
                }
                else if (rule is ValidRelativeRule<TModel> relationRule)
                {
                    if ((validationStrategy == ValidationStrategy.Force) ||
                        (memberValue.HasValue && !relationRule.IsValid(model)))
                    {
                        error = relationRule.Error ?? rulesExecutionContext.RulesOptions.DefaultError;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Unknown (or not allowed) rule in {nameof(ValidNullableRule)}");
                }

                if (error == null)
                {
                    continue;
                }

                if (result == null)
                {
                    result = new ErrorsCollection();
                }

                if (memberSpecification.SummaryError != null)
                {
                    result.AddError(memberSpecification.SummaryError);

                    break;
                }

                result.AddError(error);

                if (validationStrategy == ValidationStrategy.FailFast)
                {
                    break;
                }
            }

            return GetOrEmpty(result);
        }

        public static IErrorsCollection ExecuteCollectionMemberSpecificationRules<TModel, TItem>(IMemberSpecification memberSpecification, TModel model, IEnumerable<TItem> memberValue, IRulesExecutionContext rulesExecutionContext, int depth)
            where TModel : class
        {
            ErrorsCollection result = null;

            if (rulesExecutionContext.ValidationStrategy == ValidationStrategy.Force)
            {
                var itemErrorsCollection = ExecuteMemberSpecificationRules(
                    memberSpecification,
                    model,
                    default(TItem),
                    rulesExecutionContext,
                    depth
                );

                result = new ErrorsCollection();

                result.AddError(rulesExecutionContext.RulesOptions.CollectionForceKey, itemErrorsCollection);
            }
            else if (memberValue != null)
            {
                var items = memberValue.ToArray();

                for (var i = 0; i < items.Length; i++)
                {
                    var item = items.ElementAt(i);

                    var itemErrorsCollection = ExecuteMemberSpecificationRules(
                        memberSpecification,
                        model,
                        item,
                        rulesExecutionContext,
                        depth
                    );

                    if (itemErrorsCollection.IsEmpty)
                    {
                        continue;
                    }

                    if (result == null)
                    {
                        result = new ErrorsCollection();
                    }

                    result.AddError(i.ToString(), itemErrorsCollection);

                    if ((rulesExecutionContext.ValidationStrategy == ValidationStrategy.FailFast) && !result.IsEmpty)
                    {
                        break;
                    }
                }
            }

            return GetOrEmpty(result);
        }

        private static IErrorsCollection GetOrEmpty(ErrorsCollection result)
        {
            var anyErrors = (result != null) && !result.IsEmpty;

            return anyErrors ? result : ErrorsCollection.Empty;
        }

        private static bool IsValueType(Type type)
        {
            return !type.IsGenericType && type.IsValueType;
        }
    }
}