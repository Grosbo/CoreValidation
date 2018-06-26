using System;
using System.Collections.Generic;
using System.Reflection;
using CoreValidation.Errors;
using CoreValidation.Options;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal sealed class CompilableRule<TModel>
    {
        private readonly Func<PropertyInfo, IRulesCollection, TModel, ValidationStrategy, IValidatorsRepository, int, IRulesOptions, KeyValuePair<string, ErrorsCollection>> _compile;

        private CompilableRule(PropertyInfo propertyInfo, IRulesCollection rulesCollection, Func<PropertyInfo, IRulesCollection, TModel, ValidationStrategy, IValidatorsRepository, int, IRulesOptions, KeyValuePair<string, ErrorsCollection>> compile)
        {
            MemberPropertyInfo = propertyInfo;
            RulesCollection = rulesCollection;
            _compile = compile;
        }

        public PropertyInfo MemberPropertyInfo { get; }

        public IRulesCollection RulesCollection { get; }

        public static CompilableRule<TModel> CreateForMember(PropertyInfo propertyInfo, IRulesCollection rulesCollection, Func<PropertyInfo, IRulesCollection, TModel, ValidationStrategy, IValidatorsRepository, int, IRulesOptions, KeyValuePair<string, ErrorsCollection>> compile)
        {
            return new CompilableRule<TModel>(propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo)),
                rulesCollection ?? throw new ArgumentNullException(nameof(rulesCollection)),
                compile ?? throw new ArgumentNullException(nameof(compile)));
        }

        public static CompilableRule<TModel> CreateForSelf(Func<PropertyInfo, IRulesCollection, TModel, ValidationStrategy, IValidatorsRepository, int, IRulesOptions, KeyValuePair<string, ErrorsCollection>> compile)
        {
            return new CompilableRule<TModel>(null, null,
                compile ?? throw new ArgumentNullException(nameof(compile)));
        }

        public KeyValuePair<string, ErrorsCollection> Compile(TModel model, ValidationStrategy validationStrategy, IValidatorsRepository validatorsRepository, int depth, IRulesOptions rulesOptions)
        {
            return _compile(MemberPropertyInfo, RulesCollection, model, validationStrategy, validatorsRepository, depth, rulesOptions);
        }
    }
}