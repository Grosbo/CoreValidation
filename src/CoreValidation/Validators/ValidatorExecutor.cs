using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Specifications.Rules;

namespace CoreValidation.Validators
{
    internal static class ValidatorExecutor
    {
        public static IErrorsCollection Execute<TModel>(IValidator<TModel> validator, TModel model, IExecutionContext executionContext, int depth)
            where TModel : class
        {
            if (depth > executionContext.ExecutionOptions.MaxDepth)
            {
                throw new MaxDepthExceededException(executionContext.ExecutionOptions.MaxDepth);
            }

            ErrorsCollection errorsCollection = null;

            var validationStrategy = executionContext.ValidationStrategy;

            if ((validationStrategy == ValidationStrategy.Complete) && (validator.SummaryError != null))
            {
                validationStrategy = ValidationStrategy.FailFast;
            }

            foreach (var scope in validator.Scopes)
            {
                if ((errorsCollection != null) && !errorsCollection.IsEmpty && (validationStrategy == ValidationStrategy.FailFast))
                {
                    break;
                }

                if (scope.TryGetErrors(model, executionContext, depth, out var ruleErrorsCollection))
                {
                    if (errorsCollection == null)
                    {
                        errorsCollection = new ErrorsCollection();
                    }

                    scope.InsertErrors(errorsCollection, ruleErrorsCollection);
                }
            }

            if ((errorsCollection != null) && !errorsCollection.IsEmpty && (validator.SummaryError != null))
            {
                var summaryErrorCollection = new ErrorsCollection();
                summaryErrorCollection.AddError(validator.SummaryError);

                return summaryErrorCollection;
            }

            return errorsCollection ?? ErrorsCollection.Empty;
        }

        public static IErrorsCollection ExecuteMember<TModel, TMember>(IMemberValidator memberValidator, TModel model, TMember memberValue, IExecutionContext executionContext, int depth)
            where TModel : class
        {
            ErrorsCollection result = null;

            // ReSharper disable once CompareNonConstrainedGenericWithNull
            var exists = IsValueType(typeof(TMember)) || (memberValue != null);

            var validationStrategy = executionContext.ValidationStrategy;

            var includeRequired = !memberValidator.IsOptional && (!exists || (validationStrategy == ValidationStrategy.Force));

            if (includeRequired)
            {
                result = new ErrorsCollection();
                result.AddError(memberValidator.RequiredError ?? executionContext.ExecutionOptions.RequiredError);
            }

            if ((validationStrategy == ValidationStrategy.Complete) && (memberValidator.SummaryError != null))
            {
                validationStrategy = ValidationStrategy.FailFast;
            }

            if ((validationStrategy == ValidationStrategy.FailFast) && (result != null) && !result.IsEmpty)
            {
                return result;
            }

            if (exists || (validationStrategy == ValidationStrategy.Force))
            {
                foreach (var rule in memberValidator.Rules)
                {
                    bool anyErrors;

                    IErrorsCollection ruleErrorsCollection;

                    if (rule is ValidRule<TMember> validateRule)
                    {
                        anyErrors = validateRule.TryGetErrors(memberValue, executionContext, out ruleErrorsCollection);
                    }
                    else if (rule is ValidRelativeRule<TModel> validateRelationRule)
                    {
                        anyErrors = validateRelationRule.TryGetErrors(model, executionContext, out ruleErrorsCollection);
                    }
                    else if (rule is ValidModelRule validModelRule)
                    {
                        anyErrors = validModelRule.TryGetErrors(memberValue, executionContext, depth, out ruleErrorsCollection);
                    }
                    else if (rule is ValidCollectionRule validCollectionRule)
                    {
                        anyErrors = validCollectionRule.TryGetErrors(model, memberValue, executionContext, depth, out ruleErrorsCollection);
                    }
                    else if (rule is ValidNullableRule validNullableRule)
                    {
                        anyErrors = validNullableRule.TryGetErrors(model, memberValue, executionContext, out ruleErrorsCollection);
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

            if (memberValidator.SummaryError == null)
            {
                return result;
            }

            var summaryErrorResult = new ErrorsCollection();

            if (includeRequired)
            {
                summaryErrorResult.AddError(executionContext.ExecutionOptions.RequiredError ?? memberValidator.RequiredError);
            }

            summaryErrorResult.AddError(memberValidator.SummaryError);

            return summaryErrorResult;
        }

        public static IErrorsCollection ExecuteNullableMember<TModel, TMember>(IMemberValidator memberValidator, TModel model, TMember? memberValue, IExecutionContext executionContext)
            where TModel : class
            where TMember : struct
        {
            ErrorsCollection result = null;

            var validationStrategy = executionContext.ValidationStrategy;

            if ((validationStrategy == ValidationStrategy.Complete) && (memberValidator.SummaryError != null))
            {
                validationStrategy = ValidationStrategy.FailFast;
            }

            foreach (var rule in memberValidator.Rules)
            {
                Error error = null;

                if (rule is ValidRule<TMember> validateRule)
                {
                    if ((validationStrategy == ValidationStrategy.Force) ||
                        (memberValue.HasValue && !validateRule.IsValid(memberValue.Value)))
                    {
                        error = validateRule.Error ?? executionContext.ExecutionOptions.DefaultError;
                    }
                }
                else if (rule is ValidRelativeRule<TModel> relationRule)
                {
                    if ((validationStrategy == ValidationStrategy.Force) ||
                        (memberValue.HasValue && !relationRule.IsValid(model)))
                    {
                        error = relationRule.Error ?? executionContext.ExecutionOptions.DefaultError;
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

                if (memberValidator.SummaryError != null)
                {
                    result.AddError(memberValidator.SummaryError);

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

        public static IErrorsCollection ExecuteCollectionMember<TModel, TItem>(IMemberValidator memberValidator, TModel model, IEnumerable<TItem> memberValue, IExecutionContext executionContext, int depth)
            where TModel : class
        {
            ErrorsCollection result = null;

            if (executionContext.ValidationStrategy == ValidationStrategy.Force)
            {
                var itemErrorsCollection = ExecuteMember(
                    memberValidator,
                    model,
                    default(TItem),
                    executionContext,
                    depth
                );

                result = new ErrorsCollection();

                result.AddError(executionContext.ExecutionOptions.CollectionForceKey, itemErrorsCollection);
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

                    if ((executionContext.ValidationStrategy == ValidationStrategy.FailFast) && !result.IsEmpty)
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