using CoreValidation.Validators;

namespace CoreValidation.Factory.Validators
{
    public interface IValidatorHolder
    {
    }

    public interface IValidatorHolder<T> : IValidatorHolder
        where T : class
    {
        Validator<T> Validator { get; }
    }
}