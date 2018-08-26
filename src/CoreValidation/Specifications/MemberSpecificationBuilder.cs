using System;
using System.Collections.Generic;
using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    internal sealed class MemberSpecificationBuilder<TModel, TMember> : IMemberSpecificationBuilder<TModel, TMember>, IMemberSpecification
        where TModel : class
    {
        private readonly List<IRule> _rules = new List<IRule>();

        public IMemberSpecificationBuilder<TModel, TMember> ValidRelative(Predicate<TModel> isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null)
        {
            if (isValid == null)
            {
                throw new ArgumentNullException(nameof(isValid));
            }

            AddRule(new ValidRelativeRule<TModel>(isValid, Error.CreateValidOrNull(message, args)));

            return this;
        }

        public IMemberSpecificationBuilder<TModel, TMember> Valid(Predicate<TMember> isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null)
        {
            if (isValid == null)
            {
                throw new ArgumentNullException(nameof(isValid));
            }

            AddRule(new ValidRule<TMember>(isValid, Error.CreateValidOrNull(message, args)));

            return this;
        }

        public IMemberSpecificationBuilder<TModel, TMember> WithSummaryError(string message, IReadOnlyCollection<IMessageArg> args = null)
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

        public IMemberSpecificationBuilder<TModel, TMember> WithName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty or whitespace", nameof(name));
            }

            if (Name != null)
            {
                throw new InvalidOperationException($"{nameof(WithName)} can be set only once");
            }

            Name = name;

            return this;
        }

        public IReadOnlyCollection<IRule> Rules
        {
            get => _rules;
        }

        public Error SummaryError { get; private set; }

        public string Name { get; private set; }

        public Error RequiredError { get; private set; }

        public bool IsOptional { get; private set; }

        public void AddRule(IRule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            _rules.Add(rule);
        }

        internal IMemberSpecificationBuilder<TModel, TMember> WithRequiredError(string message, IReadOnlyCollection<IMessageArg> args = null)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (RequiredError != null)
            {
                throw new InvalidOperationException($"{nameof(WithRequiredError)} can be set only once");
            }

            RequiredError = new Error(message, args);

            return this;
        }

        internal IMemberSpecificationBuilder<TModel, TMember> Optional()
        {
            if (IsOptional)
            {
                throw new InvalidOperationException($"{nameof(Optional)} can be set only once");
            }

            IsOptional = true;

            return this;
        }
    }
}