
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CoreValidation.Specifications.Commands;

namespace CoreValidation.Specifications
{
    internal sealed class SpecificationBuilder<TModel> : ISpecificationBuilder<TModel>
        where TModel : class
    {
        private List<ICommand> _commands;


        public IReadOnlyCollection<ICommand> Commands => _commands != null
            ? (IReadOnlyCollection<ICommand>)_commands
            : Array.Empty<ICommand>();

        public ISpecificationBuilder<TModel> Member<TMember>(Expression<Func<TModel, TMember>> memberSelector, MemberSpecification<TModel, TMember> memberSpecification = null)
        {
            if (memberSelector == null)
            {
                throw new ArgumentNullException(nameof(memberSelector));
            }

            var memberScope = new MemberScope<TModel, TMember>(GetPropertyInfo(memberSelector), memberSpecification);

            AddCommand(memberScope);

            return this;
        }

        public ISpecificationBuilder<TModel> Valid(Predicate<TModel> isValid)
        {
            if (isValid == null)
            {
                throw new ArgumentNullException(nameof(isValid));
            }

            var modelScope = new ModelScope<TModel>(isValid);

            AddCommand(modelScope);

            return this;
        }

        public ISpecificationBuilder<TModel> SetSingleError(string message)
        {
            AddCommand(new SetSingleErrorCommand(message));

            return this;
        }

        public ISpecificationBuilder<TModel> WithMessage(string message)
        {
            AddCommand(new WithMessageCommand(message));

            return this;
        }

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

        public void AddCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (_commands == null)
            {
                _commands = new List<ICommand>();
            }

            _commands.Add(command);
        }
    }
}