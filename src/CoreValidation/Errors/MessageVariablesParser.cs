using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoreValidation.Errors
{
    internal class MessageVariablesParser
    {
        private static readonly Regex _curlyBracketsRegex = new Regex(@"(?<=\{)[^}]*(?=\})", RegexOptions.Compiled);

        public static char Divider { get; } = '|';

        public static char Assignment { get; } = '=';

        public IReadOnlyDictionary<string, MessageVariable> Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var matches = _curlyBracketsRegex.Matches(input)
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToList();

            var variables = new Dictionary<string, MessageVariable>();

            foreach (var match in matches)
            {
                var parts = match.Split(Divider);

                var name = parts.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                var key = $"{{{match}}}";

                if (parts.Length == 1)
                {
                    variables.Add(key, new MessageVariable {Name = name});
                }
                else
                {
                    Dictionary<string, string> parameters = null;

                    var invalidPart = false;

                    for (var i = 1; i < parts.Length; ++i)
                    {
                        var item = parts.ElementAt(i);

                        if (!item.Contains(Assignment))
                        {
                            invalidPart = true;

                            break;
                        }

                        var groups = item.Split(Assignment).Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();

                        if (groups.Length != 2)
                        {
                            invalidPart = true;

                            break;
                        }

                        if (parameters == null)
                        {
                            parameters = new Dictionary<string, string>();
                        }

                        if (parameters.ContainsKey(groups.ElementAt(0)))
                        {
                            invalidPart = true;

                            break;
                        }
                        parameters.Add(groups.ElementAt(0), groups.ElementAt(1));
                    }

                    if (invalidPart)
                    {
                        continue;
                    }

                    variables.Add(key, new MessageVariable {Name = name, Parameters = parameters});
                }
            }

            return variables;
        }
    }
}