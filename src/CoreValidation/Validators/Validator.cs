using System;
using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Specifications.Commands;

namespace CoreValidation.Validators
{
    internal class Validator<TModel> : IValidator<TModel> where TModel : class
    {
        private List<IScope<TModel>> _scopes;

        public IReadOnlyCollection<IScope<TModel>> Scopes => (IReadOnlyCollection<IScope<TModel>>)_scopes ?? Array.Empty<IScope<TModel>>();

        public IError SingleError { get; set; }

        public void AddScope(IScope<TModel> scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            if (_scopes == null)
            {
                _scopes = new List<IScope<TModel>>();
            }

            _scopes.Add(scope);
        }
    }
}