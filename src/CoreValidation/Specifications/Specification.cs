using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Options;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal class Specification
    {
        protected static readonly RulesCompiler RulesCompiler = new RulesCompiler();
        protected static readonly RulesCollector RulesCollector = new RulesCollector();
    }

    internal sealed class Specification<TModel> : Specification, ISpecification<TModel>
        where TModel : class
    {
        private readonly List<CompilableRule<TModel>> _compilableRules = new List<CompilableRule<TModel>>();

        private readonly IRulesOptions _rulesOptions;

        private readonly IValidatorsRepository _validatorsRepository;

        public Specification(TModel model, IValidatorsRepository validatorsRepository, ValidationStrategy validationStrategy, int depth, IRulesOptions rulesOptions)
        {
            if (depth > rulesOptions.MaxDepth)
            {
                throw new MaxDepthExceededException(rulesOptions.MaxDepth);
            }

            _validatorsRepository = validatorsRepository ?? throw new ArgumentNullException(nameof(validatorsRepository));

            Depth = depth;

            ValidationStrategy = validationStrategy;
            Model = model;

            _rulesOptions = rulesOptions;
        }

        public Error SummaryError { get; private set; }

        public TModel Model { get; }

        public ValidationStrategy ValidationStrategy { get; }

        public int Depth { get; }

        public IReadOnlyCollection<CompilableRule<TModel>> CompilableRules
        {
            get => _compilableRules;
        }

        public ISpecification<TModel> For<TMember>(Expression<Func<TModel, TMember>> memberSelector, MemberValidator<TModel, TMember> memberValidator = null)
        {
            if (memberSelector == null)
            {
                throw new ArgumentNullException(nameof(memberSelector));
            }

            var memberCompilableRule = CompilableRule<TModel>.CreateForMember(
                memberSelector.GetPropertyInfo(),
                RulesCollector.GetMemberRules(memberValidator ?? (c => c)),
                (memberPropertyInfo, rulesCollection, model, validationStrategy, validatorsRepository, depth, rulesOptions) =>
                {
                    var memberValue = (TMember) memberPropertyInfo.GetValue(Model);

                    var memberName = rulesCollection.Name ?? memberPropertyInfo.Name;

                    var errorsCollection = RulesCompiler.Compile(
                        rulesCollection,
                        model,
                        memberValue,
                        validatorsRepository,
                        validationStrategy,
                        depth,
                        rulesOptions
                    );

                    return new KeyValuePair<string, ErrorsCollection>(memberName, errorsCollection);
                });

            _compilableRules.Add(memberCompilableRule);

            return this;
        }

        public ISpecification<TModel> Valid(Predicate<TModel> isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null)
        {
            if (isValid == null)
            {
                throw new ArgumentNullException(nameof(isValid));
            }

            if ((message == null) && (args != null))
            {
                throw new InvalidOperationException($"Passing {nameof(args)} is allowed only if {nameof(message)} is present");
            }

            var selfCompilableRule = CompilableRule<TModel>.CreateForSelf((memberPropertyInfo, rulesCollection, model, validationStrategy, validatorsRepository, depth, rulesOptions) =>
                {
                    var errorsCollection = new ErrorsCollection();

                    var error = ValidRelativeRule<TModel>.CompileError(isValid, message, args, model, validationStrategy, rulesOptions.DefaultError);

                    if (error != null)
                    {
                        errorsCollection.AddError(error);
                    }

                    return new KeyValuePair<string, ErrorsCollection>(null, errorsCollection);
                }
            );

            _compilableRules.Add(selfCompilableRule);

            return this;
        }

        public ISpecification<TModel> WithSummaryError(string message, IReadOnlyCollection<IMessageArg> args = null)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (SummaryError != null)
            {
                throw new InvalidOperationException($"{nameof(WithSummaryError)} can be set only once");
            }

            SummaryError = new Error(message, args);

            return this;
        }

        public ErrorsCollection GetErrors()
        {
            var errorsCollection = new ErrorsCollection();

            var validationStrategy = ValidationStrategy;

            if ((ValidationStrategy == ValidationStrategy.Complete) && (SummaryError != null))
            {
                validationStrategy = ValidationStrategy.FailFast;
            }

            foreach (var cacheItem in _compilableRules)
            {
                if ((ValidationStrategy == ValidationStrategy.FailFast) && !errorsCollection.IsEmpty)
                {
                    break;
                }

                var itemErrors = cacheItem.Compile(Model, validationStrategy, _validatorsRepository, Depth, _rulesOptions);

                if (!itemErrors.Value.IsEmpty)
                {
                    if (itemErrors.Key == null)
                    {
                        errorsCollection.AddError(itemErrors.Value.Errors.Single());
                    }
                    else
                    {
                        errorsCollection.AddError(itemErrors.Key, itemErrors.Value);
                    }
                }
            }

            if (!errorsCollection.IsEmpty && (SummaryError != null))
            {
                var summaryErrorCollection = new ErrorsCollection();
                summaryErrorCollection.AddError(SummaryError);

                return summaryErrorCollection;
            }

            return errorsCollection;
        }
    }
}