using System.Collections.Generic;

namespace CoreValidation.Errors
{
    public sealed class MessageVariable
    {
        public string Name { get; set; }

        public IReadOnlyDictionary<string, string> Parameters { get; set; }
    }
}