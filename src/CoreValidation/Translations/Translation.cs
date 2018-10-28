using System;
using System.Collections.Generic;

namespace CoreValidation.Translations
{
    /// <summary>
    /// Translation for the messages
    /// </summary>
    public sealed class Translation
    {
        /// <summary>
        /// Initializes new instance of <see cref="Translation"/>.
        /// </summary>
        /// <param name="name">Name of the translation.</param>
        /// <param name="dictionary">Dictionary with all translation entries. Keys are the original phrases. Values are the translations.</param>
        /// <exception cref="ArgumentNullException">Throw if <paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throw if <paramref name="dictionary"/> is null.</exception>
        public Translation(string name, IDictionary<string, string> dictionary)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        /// <summary>
        /// Name of the translation.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Dictionary with all translation entries. Keys are the original phrases. Values are the translations.
        /// </summary>
        public IDictionary<string, string> Dictionary { get; }
    }
}