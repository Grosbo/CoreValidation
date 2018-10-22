using System;
using System.Collections.Generic;

namespace CoreValidation.Errors.Args
{
    public class TextArg : IMessageArg<string>
    {
        private static readonly string _caseParameter = "case";

        private static readonly string _upperCaseParameterValue = "upper";

        private static readonly string _lowerCaseParameterValue = "lower";

        public TextArg(string name, string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public TextArg(string name, char value) : this(name, value.ToString())
        {
        }

        public string Name { get; }

        public string Value { get; }

        public IReadOnlyCollection<string> AllowedParameters { get; } = new[] {_caseParameter};

        public string ToString(IReadOnlyDictionary<string, string> parameters)
        {
            var caseParameter = parameters?.ContainsKey(_caseParameter) == true
                ? parameters[_caseParameter]
                : null;

            if ((caseParameter != null) &&
                (caseParameter != _upperCaseParameterValue) &&
                (caseParameter != _lowerCaseParameterValue))
            {
                caseParameter = null;
            }

            return Stringify(Value, caseParameter);
        }

        private static string Stringify(string value, string caseParameter)
        {
            if (caseParameter == null)
            {
                return value;
            }

            if (caseParameter == _upperCaseParameterValue)
            {
                return value.ToUpper();
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (caseParameter == _lowerCaseParameterValue)
            {
                return value.ToLower();
            }

            return value;
        }
    }
}