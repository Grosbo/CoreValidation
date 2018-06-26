using System.Collections.Generic;

namespace CoreValidation.Translations.Template
{
    internal class TranslationTemplateService
    {
        public IReadOnlyDictionary<string, string> CreateDictionary(ITranslationTemplate source)
        {
            var dictionary = new Dictionary<string, string>();

            var templateProperties = typeof(ITranslationTemplate).GetProperties();

            foreach (var templateProperty in templateProperties)
            {
                if (templateProperty.PropertyType == typeof(string))
                {
                    dictionary.Add(templateProperty.Name, (string)templateProperty.GetValue(source));
                }
                else
                {
                    var group = templateProperty.GetValue(source);

                    var groupProperties = templateProperty.PropertyType.GetProperties();

                    foreach (var groupProperty in groupProperties)
                    {
                        var value = (string)groupProperty.GetValue(group);

                        dictionary.Add($"{templateProperty.Name}.{groupProperty.Name}", value);
                    }
                }
            }

            return dictionary;
        }

        public ITranslationTemplate CreateKeysTemplate()
        {
            return new KeysTemplate();
        }

        private class KeysTemplate : ITranslationTemplate
        {
            public KeysTemplate()
            {
                Collections = Assign(nameof(Collections), new CollectionsMessages());
                Bool = Assign(nameof(Bool), new BoolMessages());
                Numbers = Assign(nameof(Numbers), new NumbersMessages());
                Texts = Assign(nameof(Texts), new TextsMessages());
                Times = Assign(nameof(Times), new TimesMessages());
                TimeSpan = Assign(nameof(TimeSpan), new TimeSpanMessages());
                Char = Assign(nameof(TimeSpan), new CharMessages());
                Required = nameof(Required);
            }

            public ICollectionsMessages Collections { get; }
            public IBoolMessages Bool { get; }
            public INumbersMessages Numbers { get; }
            public ITimeSpanMessages TimeSpan { get; }
            public ICharMessages Char { get; }
            public ITextsMessages Texts { get; }
            public ITimesMessages Times { get; }
            public string Required { get; }

            private T Assign<T>(string prefix, T target)
            {
                var properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    property.SetValue(target, $"{prefix}.{property.Name}");
                }

                return target;
            }
        }
    }
}