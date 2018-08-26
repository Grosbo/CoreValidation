using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    internal class RuleExecutionResult
    {
        public bool IsModelLevel => MemberName == null;

        public bool IsValid => (ErrorsCollection == null) || ErrorsCollection.IsEmpty;

        public string MemberName { get; set; }
        public IErrorsCollection ErrorsCollection { get; set; }


        public static RuleExecutionResult Valid { get; } = new RuleExecutionResult();
    }
}