using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal sealed class SpecificationBuilder<TModel> : ISpecificationBuilder<TModel>, ISpecification<TModel>
        where TModel : class
    {
        private readonly List<IExecutableRule<TModel>> _executableRules = new List<IExecutableRule<TModel>>();

        public ISpecificationBuilder<TModel> For<TMember>(Expression<Func<TModel, TMember>> memberSelector, MemberValidator<TModel, TMember> memberValidator = null)
        {
            if (memberSelector == null)
            {
                throw new ArgumentNullException(nameof(memberSelector));
            }

            var executableMemberRule = new ExecutableMemberRule<TModel, TMember>(memberSelector.GetPropertyInfo(), memberValidator);

            _executableRules.Add(executableMemberRule);

            return this;
        }

        public ISpecificationBuilder<TModel> Valid(Predicate<TModel> isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null)
        {
            if (isValid == null)
            {
                throw new ArgumentNullException(nameof(isValid));
            }

            var executableSelfRule = new ExecutableSelfRule<TModel>(isValid, Error.CreateValidOrNull(message, args));

            _executableRules.Add(executableSelfRule);

            return this;
        }

        public ISpecificationBuilder<TModel> WithSummaryError(string message, IReadOnlyCollection<IMessageArg> args = null)
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

        public Error SummaryError { get; private set; }

        public IReadOnlyCollection<IExecutableRule<TModel>> ExecutableRules => _executableRules;
    }
}