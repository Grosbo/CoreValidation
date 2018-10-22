using System;
using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Specifications.Commands;

namespace CoreValidation.Validators
{
    internal class MemberValidator : IMemberValidator
    {
        private List<IRule> _rules;

        public IReadOnlyCollection<IRule> Rules => (IReadOnlyCollection<IRule>)_rules ?? Array.Empty<IRule>();

        public bool IsOptional { get; set; }
        public IError SingleError { get; set; }
        public IError RequiredError { get; set; }

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
    }
}