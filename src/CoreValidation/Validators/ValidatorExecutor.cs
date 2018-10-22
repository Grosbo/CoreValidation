using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Specifications.Commands;

namespace CoreValidation.Validators
{
    internal static class ValidatorExecutor
    {
        public static IErrorsCollection Execute<TModel>(IValidator<TModel> validator, TModel model, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth)
            where TModel : class
        {
            if (depth > executionContext.MaxDepth)
            {
                throw new MaxDepthExceededException(executionContext.MaxDepth);
            }

            ErrorsCollection errorsCollection = null;

            var strategy = validationStrategy;

            if ((strategy == ValidationStrategy.Complete) && (validator.SingleError != null))
            {
                strategy = ValidationStrategy.FailFast;
            }

            foreach (var scope in validator.Scopes)
            {
                if ((errorsCollection != null) && !errorsCollection.IsEmpty && (strategy == ValidationStrategy.FailFast))
                {
                    break;
                }

                if (scope.TryGetErrors(model, executionContext, strategy, depth, out var ruleErrorsCollection))
                {
                    if (errorsCollection == null)
                    {
                        errorsCollection = new ErrorsCollection();
                    }

                    scope.InsertErrors(errorsCollection, ruleErrorsCollection);
                }
            }

            if ((errorsCollection != null) && !errorsCollection.IsEmpty && (validator.SingleError != null))
            {
                var summaryErrorCollection = new ErrorsCollection();
                summaryErrorCollection.AddError(validator.SingleError);

                return summaryErrorCollection;
            }

            return errorsCollection ?? ErrorsCollection.Empty;
        }

        public static IErrorsCollection ExecuteMember<TModel, TMember>(IMemberValidator memberValidator, TModel model, TMember memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth)
            where TModel : class
        {
            ErrorsCollection result = null;

            // ReSharper disable once CompareNonConstrainedGenericWithNull
            var exists = IsValueType(typeof(TMember)) || (memberValue != null);

            var strategy = validationStrategy;

            var includeRequired = !memberValidator.IsOptional && (!exists || (strategy == ValidationStrategy.Force));

            if (includeRequired)
            {
                result = new ErrorsCollection();
                result.AddError(memberValidator.RequiredError ?? executionContext.RequiredError);
            }

            if ((strategy == ValidationStrategy.Complete) && (memberValidator.SingleError != null))
            {
                strategy = ValidationStrategy.FailFast;
            }

            if ((strategy == ValidationStrategy.FailFast) && (result != null) && !result.IsEmpty)
            {
                return result;
            }

            if (exists || (strategy == ValidationStrategy.Force))
            {
                foreach (var rule in memberValidator.Rules)
                {
                    bool anyErrors;

                    IErrorsCollection ruleErrorsCollection;

                    if (rule is ValidRule<TMember> validateRule)
                    {
                        anyErrors = validateRule.TryGetErrors(memberValue, executionContext, strategy, out ruleErrorsCollection);
                    }
                    else if (rule is AsRelativeRule<TModel> validateRelationRule)
                    {
                        anyErrors = validateRelationRule.TryGetErrors(model, executionContext, strategy, out ruleErrorsCollection);
                    }
                    else if (rule is AsModelRule validModelRule)
                    {
                        anyErrors = validModelRule.TryGetErrors(memberValue, executionContext, strategy, depth, out ruleErrorsCollection);
                    }
                    else if (rule is AsCollectionRule validCollectionRule)
                    {
                        anyErrors = validCollectionRule.TryGetErrors(model, memberValue, executionContext, strategy, depth, out ruleErrorsCollection);
                    }
                    else if (rule is AsNullableRule validNullableRule)
                    {
                        anyErrors = validNullableRule.TryGetErrors(model, memberValue, executionContext, strategy, out ruleErrorsCollection);
                    }
                    else
                    {
                        throw new InvalidRuleException("Unknown rule");
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

                    if (strategy == ValidationStrategy.FailFast)
                    {
                        break;
                    }
                }
            }

            if ((result == null) || result.IsEmpty)
            {
                return ErrorsCollection.Empty;
            }

            if (memberValidator.SingleError == null)
            {
                return result;
            }

            var summaryErrorResult = new ErrorsCollection();

            if (includeRequired)
            {
                summaryErrorResult.AddError(executionContext.RequiredError ?? memberValidator.RequiredError);
            }

            summaryErrorResult.AddError(memberValidator.SingleError);

            return summaryErrorResult;
        }

        public static IErrorsCollection ExecuteNullableMember<TModel, TMember>(IMemberValidator memberValidator, TModel model, TMember? memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy)
            where TModel : class
            where TMember : struct
        {
            ErrorsCollection result = null;

            var strategy = validationStrategy;

            if ((strategy == ValidationStrategy.Complete) && (memberValidator.SingleError != null))
            {
                strategy = ValidationStrategy.FailFast;
            }

            foreach (var rule in memberValidator.Rules)
            {
                IError error = null;

                if (rule is ValidRule<TMember> validateRule)
                {
                    if ((strategy == ValidationStrategy.Force) ||
                        (memberValue.HasValue && !validateRule.IsValid(memberValue.Value)))
                    {
                        error = validateRule.RuleSingleError ?? validateRule.Error ?? executionContext.DefaultError;
                    }
                }
                else if (rule is AsRelativeRule<TModel> relationRule)
                {
                    if ((strategy == ValidationStrategy.Force) ||
                        (memberValue.HasValue && !relationRule.IsValid(model)))
                    {
                        error = relationRule.RuleSingleError ?? relationRule.Error ?? executionContext.DefaultError;
                    }
                }
                else
                {
                    throw new InvalidRuleException($"Unknown (or not allowed) rule in {nameof(AsNullableRule)}");
                }

                if (error == null)
                {
                    continue;
                }

                if (result == null)
                {
                    result = new ErrorsCollection();
                }

                if (memberValidator.SingleError != null)
                {
                    result.AddError(memberValidator.SingleError);

                    break;
                }

                result.AddError(error);

                if (strategy == ValidationStrategy.FailFast)
                {
                    break;
                }
            }

            return GetOrEmpty(result);
        }

        public static IErrorsCollection ExecuteCollectionMember<TModel, TItem>(IMemberValidator memberValidator, TModel model, IEnumerable<TItem> memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth)
            where TModel : class
        {
            ErrorsCollection result = null;

            if (validationStrategy == ValidationStrategy.Force)
            {
                var itemErrorsCollection = ExecuteMember(
                    memberValidator,
                    model,
                    default(TItem),
                    executionContext,
                    validationStrategy,
                    depth
                );

                result = new ErrorsCollection();

                result.AddError(executionContext.CollectionForceKey, itemErrorsCollection);
            }
            else if (memberValue != null)
            {
                var items = memberValue.ToArray();

                for (var i = 0; i < items.Length; i++)
                {
                    var item = items.ElementAt(i);

                    var itemErrorsCollection = ExecuteMember(
                        memberValidator,
                        model,
                        item,
                        executionContext,
                        validationStrategy,
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

                    if ((validationStrategy == ValidationStrategy.FailFast) && !result.IsEmpty)
                    {
                        break;
                    }
                }
            }

            return GetOrEmpty(result);
        }

        private static IErrorsCollection GetOrEmpty(IErrorsCollection result)
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