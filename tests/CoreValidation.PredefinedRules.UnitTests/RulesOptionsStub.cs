using CoreValidation.Errors;
using CoreValidation.Options;

namespace CoreValidation.PredefinedRules.UnitTests
{
    public class RulesOptionsStub : IRulesOptions
    {
        public string CollectionForceKey { get; set; } = "*";

        public Error RequiredError { get; set; } = new Error("Required");

        public Error DefaultError { get; set; } = new Error("Invalid");

        public int MaxDepth { get; set; } = 10;
    }
}