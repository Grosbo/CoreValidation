using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;

namespace CoreValidation.Validators
{
    internal static class MemberValidatorCreator
    {
        private static readonly IReadOnlyCollection<Type> _withMessageCommandCompatible = new[]
        {
            typeof(WithMessageCommand)
        };

        public static IMemberValidator Create<TModel, TMember>(MemberSpecification<TModel, TMember> memberSpecification)
            where TModel : class
        {
            return Create(memberSpecification, out _);
        }

        public static IMemberValidator Create<TModel, TMember>(MemberSpecification<TModel, TMember> memberSpecification, out IReadOnlyCollection<ICommand> commands)
            where TModel : class
        {
            var builder = new MemberSpecificationBuilder<TModel, TMember>();

            var processedBuilder = memberSpecification(builder);

            if (!ReferenceEquals(builder, processedBuilder))
            {
                throw new InvalidProcessedReferenceException(typeof(MemberSpecificationBuilder<TModel, TMember>));
            }

            commands = (processedBuilder as MemberSpecificationBuilder<TModel, TMember>)?.Commands;

            return ConvertCommands(commands);
        }

        private static IMemberValidator ConvertCommands(IReadOnlyCollection<ICommand> commands)
        {
            var memberValidator = new MemberValidator();

            for (var i = 0; i < commands.Count; ++i)
            {
                var command = commands.ElementAt(i);

                if (command is IRule rule)
                {
                    memberValidator.AddRule(rule);
                }
                else if (command is SetOptionalCommand)
                {
                    if (memberValidator.IsOptional)
                    {
                        throw new InvalidCommandDuplicationException(i, command.Name);
                    }

                    memberValidator.IsOptional = true;
                }
                else if (command is SetRequiredCommand setRequiredErrorCommand)
                {
                    if (memberValidator.RequiredError != null)
                    {
                        throw new InvalidCommandDuplicationException(i, command.Name);
                    }

                    memberValidator.RequiredError = new Error(setRequiredErrorCommand.Message);
                }
                else if (command is SetSingleErrorCommand setSingleErrorCommand)
                {
                    if (memberValidator.SingleError != null)
                    {
                        throw new InvalidCommandDuplicationException(i, command.Name);
                    }

                    memberValidator.SingleError = new Error(setSingleErrorCommand.Message);
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

            return memberValidator;
        }
    }
}