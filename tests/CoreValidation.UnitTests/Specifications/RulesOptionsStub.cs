using CoreValidation.Errors;
using CoreValidation.Options;

namespace CoreValidation.UnitTests.Specifications
{
    public class RulesOptionsStub : IRulesOptions
    {
        public string CollectionForceKey { get; set; } = "*";

        public Error RequiredError { get; set; } = new Error("Required");

        public int MaxDepth { get; set; } = 10;
    }
}