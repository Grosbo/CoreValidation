using CoreValidation.Specifications;

namespace CoreValidation.Validators
{
    public delegate ISpecification<T> Validator<T>(ISpecification<T> specification)
        where T: class;
}