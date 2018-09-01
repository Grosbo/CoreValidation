using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Validators;
using CoreValidation.Validators.Scopes;

namespace CoreValidation.Specifications
{
    internal sealed class SpecificationBuilder<TModel> : ISpecificationBuilder<TModel>, IValidator<TModel>
        where TModel : class
    {
        private readonly List<IValidationScope<TModel>> _scopes = new List<IValidationScope<TModel>>();

        public ISpecificationBuilder<TModel> For<TMember>(Expression<Func<TModel, TMember>> memberSelector, MemberSpecification<TModel, TMember> memberSpecification = null)
        {
            if (memberSelector == null)
            {
                throw new ArgumentNullException(nameof(memberSelector));
            }

            var memberScope = new MemberScope<TModel, TMember>(GetPropertyInfo(memberSelector), memberSpecification);

            _scopes.Add(memberScope);

            return this;
        }

        public ISpecificationBuilder<TModel> Valid(Predicate<TModel> isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null)
        {
            if (isValid == null)
            {
                throw new ArgumentNullException(nameof(isValid));
            }

            var modelScope = new ModelScope<TModel>(isValid, Error.CreateValidOrNull(message, args));

            _scopes.Add(modelScope);

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

        public IReadOnlyCollection<IValidationScope<TModel>> Scopes => _scopes;

        private static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> memberSelector)
        {
            var type = typeof(TSource);

            if (!(memberSelector.Body is MemberExpression member))
            {
                throw new ArgumentException($"Expression '{memberSelector}' refers to a method, not a property.");
            }

            var propInfo = member.Member as PropertyInfo;

            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{memberSelector}' refers to a field, not a property.");
            }

            if ((type != propInfo.ReflectedType) && ((propInfo.ReflectedType == null) || !type.IsSubclassOf(propInfo.ReflectedType)))
            {
                throw new ArgumentException($"Expression '{memberSelector}' refers to a property that is not from type {type}.");
            }

            return propInfo;
        }
    }
}