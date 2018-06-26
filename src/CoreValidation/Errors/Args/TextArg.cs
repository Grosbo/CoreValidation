using System;
using System.Collections.Generic;

namespace CoreValidation.Errors.Args
{
    public class TextArg : IMessageArg
    {
        private static readonly string _caseParameter = "case";

        private static readonly string _upperCaseParameterValue = "upper";

        private static readonly string _lowerCaseParameterValue = "lower";

        private readonly Func<string, string> _stringify;

        private TextArg(string name, Func<string, string> stringify)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _stringify = stringify ?? throw new ArgumentNullException(nameof(stringify));
        }

        public TextArg(string name, string value)
            : this(name, caseValue => Stringify(value, caseValue))
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }

        public TextArg(string name, char value)
            : this(name, caseValue => Stringify(value, caseValue))
        {
        }

        public string Name { get; }

        public IReadOnlyCollection<string> AllowedParameters { get; } = new[] {_caseParameter};

        public string ToString(IReadOnlyDictionary<string, string> parameters)
        {
            var caseValue = parameters?.ContainsKey(_caseParameter) == true
                ? parameters[_caseParameter]
                : null;

            if ((caseValue != null) &&
                (caseValue != _upperCaseParameterValue) &&
                (caseValue != _lowerCaseParameterValue))
            {
                caseValue = null;
            }

            return _stringify(caseValue);
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

        private static string Stringify(char value, string caseParameter)
        {
            if (caseParameter == null)
            {
                return value.ToString();
            }

            if (caseParameter == _upperCaseParameterValue)
            {
                return char.ToUpper(value).ToString();
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (caseParameter == _lowerCaseParameterValue)
            {
                return char.ToLower(value).ToString();
            }

            return value.ToString();
        }
    }
}