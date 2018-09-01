using System;
using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications.Rules;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal sealed class MemberSpecificationBuilder<TModel, TMember> : IMemberSpecificationBuilder<TModel, TMember>, IMemberValidator
        where TModel : class
    {
        private List<IRule> _rules;

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

        public IReadOnlyCollection<IRule> Rules => _rules != null
            ? (IReadOnlyCollection<IRule>)_rules
            : Array.Empty<IRule>();

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

            if (_rules == null)
            {
                _rules = new List<IRule>();
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