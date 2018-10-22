using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;

namespace CoreValidation.Validators
{
    internal static class ValidatorCreator
    {
        private static readonly IReadOnlyCollection<Type> _withMessageCommandCompatible = new[]
        {
            typeof(WithMessageCommand)
        };

        public static IValidator<TModel> Create<TModel>(Specification<TModel> specification)
            where TModel : class
        {
            return Create(specification, out _);
        }

        public static IValidator<TModel> Create<TModel>(Specification<TModel> specification, out IReadOnlyCollection<ICommand> commands)
            where TModel : class
        {
            var builder = new SpecificationBuilder<TModel>();

            var processedBuilder = specification(builder);

            if (!ReferenceEquals(builder, processedBuilder))
            {
                throw new InvalidProcessedReferenceException(typeof(SpecificationBuilder<TModel>));
            }

            // ReSharper disable once PossibleNullReferenceException
            commands = (processedBuilder as SpecificationBuilder<TModel>).Commands;

            return ConvertCommands<TModel>(commands);
        }

        private static IValidator<TModel> ConvertCommands<TModel>(IReadOnlyCollection<ICommand> commands)
            where TModel : class
        {
            var validator = new Validator<TModel>();

            for (var i = 0; i < commands.Count; ++i)
            {
                var command = commands.ElementAt(i);

                if (command is IScope<TModel> scope)
                {
                    validator.AddScope(scope);
                }
                else if (command is SetSingleErrorCommand setSingleErrorCommand)
                {
                    if (validator.SingleError != null)
                    {
                        throw new InvalidCommandDuplicationException(i, command.Name);
                    }

                    validator.SingleError = new Error(setSingleErrorCommand.Message);
                }
                else if (command is WithMessageCommand withMessageCommand)
                {
                    if (i == 0)
                    {
                        throw new InvalidCommandOrderException($"Command {withMessageCommand.Name} is invalid at the beginning", i, withMessageCommand.Name);
                    }

                    var singleErrorHolder = CommandsHelper.GetClosestCommand<IRule>(commands, i, _withMessageCommandCompatible);

                    singleErrorHolder.RuleSingleError = new Error(withMessageCommand.Message);
                }
            }

            return validator;
        }
    }
}