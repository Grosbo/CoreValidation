using System;
using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications.Commands;

namespace CoreValidation.Specifications
{
    internal sealed class MemberSpecificationBuilder<TModel, TMember> : IMemberSpecificationBuilder<TModel, TMember>
        where TModel : class
    {
        private List<ICommand> _commands;

        public IReadOnlyCollection<ICommand> Commands => _commands != null
            ? (IReadOnlyCollection<ICommand>)_commands
            : Array.Empty<ICommand>();

        public IMemberSpecificationBuilder<TModel, TMember> AsRelative(Predicate<TModel> isValid)
        {
            if (isValid == null)
            {
                throw new ArgumentNullException(nameof(isValid));
            }

            AddCommand(new AsRelativeRule<TModel>(isValid));

            return this;
        }

        public IMemberSpecificationBuilder<TModel, TMember> Valid(Predicate<TMember> isValid)
        {
            if (isValid == null)
            {
                throw new ArgumentNullException(nameof(isValid));
            }

            AddCommand(new ValidRule<TMember>(isValid));

            return this;
        }

        public IMemberSpecificationBuilder<TModel, TMember> Valid(Predicate<TMember> isValid, string message, IReadOnlyCollection<IMessageArg> args = null)
        {
            if (isValid == null)
            {
                throw new ArgumentNullException(nameof(isValid));
            }

            AddCommand(new ValidRule<TMember>(isValid, Error.CreateValidOrNull(message, args)));

            return this;
        }

        public IMemberSpecificationBuilder<TModel, TMember> SetSingleError(string message)
        {
            AddCommand(new SetSingleErrorCommand(message));

            return this;
        }

        public IMemberSpecificationBuilder<TModel, TMember> WithMessage(string message)
        {
            AddCommand(new WithMessageCommand(message));

            return this;
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

        internal IMemberSpecificationBuilder<TModel, TMember> SetRequired(string message)
        {
            AddCommand(new SetRequiredCommand(message));

            return this;
        }

        internal IMemberSpecificationBuilder<TModel, TMember> SetOptional()
        {
            AddCommand(new SetOptionalCommand());

            return this;
        }
    }
}