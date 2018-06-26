using System;
using CoreValidation.Errors;
using CoreValidation.Options;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal sealed class RulesCompiler
    {
        public ErrorsCollection Compile<TModel, TMember>(IRulesCollection rulesCollection, TModel model, TMember memberValue, IValidatorsRepository validatorsRepository, ValidationStrategy validationStrategy, int depthLevel, IRulesOptions rulesOptions)
            where TModel : class
        {
            var errorsCollection = new ErrorsCollection();

            // ReSharper disable once CompareNonConstrainedGenericWithNull
            var exists = IsValueType(typeof(TMember)) || (memberValue != null);

            var shouldIncludeRequiredError = !rulesCollection.IsOptional && (!exists || (validationStrategy == ValidationStrategy.Force));

            if (shouldIncludeRequiredError)
            {
                errorsCollection.AddError(rulesCollection.RequiredError ?? rulesOptions.RequiredError.Clone());
            }

            if ((validationStrategy == ValidationStrategy.Complete) && (rulesCollection.SummaryError != null))
            {
                validationStrategy = ValidationStrategy.FailFast;
            }

            if ((validationStrategy == ValidationStrategy.FailFast) && !errorsCollection.IsEmpty)
            {
                return errorsCollection;
            }

            if (exists || (validationStrategy == ValidationStrategy.Force))
            {
                foreach (var rule in rulesCollection.Rules)
                {
                    if (TryCompileSingleErrorRules(model, memberValue, rule, validationStrategy, out var error))
                    {
                        if (error == null)
                        {
                            continue;
                        }

                        errorsCollection.AddError(error);

                        if (validationStrategy == ValidationStrategy.FailFast)
                        {
                            break;
                        }
                    }
                    else if (TryCompileNestedRules(model, memberValue, rule, validatorsRepository, validationStrategy, depthLevel, rulesOptions, out var ruleErrorsCollection))
                    {
                        if (ruleErrorsCollection.IsEmpty)
                        {
                            continue;
                        }

                        errorsCollection.Include(ruleErrorsCollection);

                        if (validationStrategy == ValidationStrategy.FailFast)
                        {
                            break;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Unknown rule");
                    }
                }
            }

            return GetFinalizedErrorsCollection(errorsCollection, rulesCollection.SummaryError, shouldIncludeRequiredError, rulesOptions.RequiredError, rulesCollection.RequiredError);
        }

        private bool IsValueType(Type type)
        {
            return !type.IsGenericType && type.IsValueType;
        }

        private ErrorsCollection GetFinalizedErrorsCollection(ErrorsCollection errorsCollection, Error error, bool shouldIncludeRequiredError, Error defaultRequiredErrorMessage, Error requiredErrorMessage)
        {
            if (errorsCollection.IsEmpty || (error == null))
            {
                return errorsCollection;
            }

            var singleErrorCollection = new ErrorsCollection();

            if (shouldIncludeRequiredError)
            {
                singleErrorCollection.AddError(requiredErrorMessage ?? defaultRequiredErrorMessage.Clone());
            }

            singleErrorCollection.AddError(error);

            return singleErrorCollection;
        }

        private bool TryCompileSingleErrorRules<TModel, TMember>(TModel model, TMember memberValue, IRule rule, ValidationStrategy validationStrategy, out Error error)
            where TModel : class
        {
            if (rule is ValidRule<TMember> validateRule)
            {
                error = validateRule.CompileError(memberValue, validationStrategy);
            }
            else if (rule is ValidRelativeRule<TModel> validateRelationRule)
            {
                error = validateRelationRule.CompileError(model, validationStrategy);
            }
            else
            {
                error = null;

                return false;
            }

            return true;
        }

        private bool TryCompileNestedRules<TModel, TMember>(TModel model, TMember memberValue, IRule rule, IValidatorsRepository validatorsRepository, ValidationStrategy validationStrategy, int depth, IRulesOptions rulesOptions, out ErrorsCollection errorsCollection)
        {
            if (rule is ValidModelRule)
            {
                errorsCollection = rule.Compile(new object[]
                {
                    memberValue,
                    validatorsRepository,
                    validationStrategy,
                    depth,
                    rulesOptions
                });
            }
            else if (rule is ValidCollectionRule)
            {
                errorsCollection = rule.Compile(new object[]
                {
                    model,
                    memberValue,
                    validatorsRepository,
                    validationStrategy,
                    depth,
                    rulesOptions
                });
            }
            else if (rule is ValidNullableRule)
            {
                errorsCollection = rule.Compile(new object[]
                {
                    model,
                    memberValue,
                    validationStrategy
                });
            }
            else
            {
                errorsCollection = null;

                return false;
            }

            return true;
        }
    }
}