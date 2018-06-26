using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    public interface IRule
    {
        ErrorsCollection Compile(object[] args);
    }
}