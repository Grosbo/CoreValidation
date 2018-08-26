using System;
using CoreValidation.Errors;
using CoreValidation.Options;

namespace CoreValidation.Specifications
{
    internal class RulesExecutionContext : IRulesExecutionContext
    {
        private readonly Lazy<ErrorsCollection> _defaultErrorCollection;


        public RulesExecutionContext()
        {
            _defaultErrorCollection = new Lazy<ErrorsCollection>(() =>
            {
                if (RulesOptions?.DefaultError == null)
                {
                    return null;
                }

                var defaultErrorCollection = new ErrorsCollection();
                defaultErrorCollection.AddError(RulesOptions.DefaultError);

                return defaultErrorCollection;
            });
        }

        public IRulesOptions RulesOptions { get; set; }
        public ISpecificationsRepository SpecificationsRepository { get; set; }
        public ValidationStrategy ValidationStrategy { get; set; }

        public ErrorsCollection DefaultErrorCollection
        {
            get => _defaultErrorCollection.Value;
        }
    }
}