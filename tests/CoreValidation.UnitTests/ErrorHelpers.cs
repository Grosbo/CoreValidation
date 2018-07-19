using CoreValidation.Errors;

namespace CoreValidation.UnitTests
{
    public static class ErrorHelpers
    {
        public static Error DefaultErrorStub { get; } = new Error("Required");
    }
}