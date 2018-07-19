using CoreValidation.Errors;
using CoreValidation.Options;

namespace CoreValidation.UnitTests.Results
{
    public class RulesOptionsStub : IRulesOptions
    {
        public string CollectionForceKey { get; set; }
        public Error RequiredError { get; set; }
        public Error DefaultError { get; set; }
        public int MaxDepth { get; set; }
    }
}