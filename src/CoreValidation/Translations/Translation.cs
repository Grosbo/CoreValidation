using System;
using System.Collections.Generic;

namespace CoreValidation.Translations
{
    public sealed class Translation
    {
        public Translation(string name, IDictionary<string, string> dictionary)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        public string Name { get; }

        public IDictionary<string, string> Dictionary { get; }
    }
}