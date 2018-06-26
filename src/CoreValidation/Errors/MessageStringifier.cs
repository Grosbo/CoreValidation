using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreValidation.Errors
{
    internal class MessageStringifier
    {
        private static readonly MessageVariablesParser _messageVariablesParser = new MessageVariablesParser();

        public string Stringify(string message, IReadOnlyCollection<IMessageArg> messageArgs)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var messageBuilder = new StringBuilder(message);

            if ((messageArgs == null) || !messageArgs.Any())
            {
                return message;
            }

            if (messageArgs.Contains(null))
            {
                throw new ArgumentNullException(nameof(messageArgs), "Contains null");
            }

            var uniqueNames = messageArgs.Select(m => m.Name).Distinct().Count();

            if (uniqueNames != messageArgs.Count)
            {
                throw new ArgumentException($"Duplicate {nameof(IMessageArg.Name)} in {nameof(messageArgs)}");
            }

            var messageVariables = _messageVariablesParser.Parse(message);

            foreach (var messageVariable in messageVariables)
            {
                var arg = messageArgs.SingleOrDefault(a => a.Name == messageVariable.Value.Name);

                if (arg == null)
                {
                    continue;
                }

                var withInvalidParameters =
                    (messageVariable.Value.Parameters != null) &&
                    (messageVariable.Value.Parameters.Count > 0) &&
                    messageVariable.Value.Parameters.Keys.Any(param => !arg.AllowedParameters.Contains(param));

                if (withInvalidParameters)
                {
                    continue;
                }

                var value = arg.ToString(messageVariable.Value.Parameters);

                messageBuilder.Replace(messageVariable.Key, value);
            }

            ReplaceNewLines(messageBuilder);

            return messageBuilder.ToString();
        }

        private static void ReplaceNewLines(StringBuilder stringBuilder)
        {
            stringBuilder.Replace(Environment.NewLine, @"↲");
        }
    }
}