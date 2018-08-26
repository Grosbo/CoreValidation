using CoreValidation.Specifications;

namespace CoreValidation.Validators
{
    public delegate ISpecificationBuilder<T> Validator<T>(ISpecificationBuilder<T> specificationBuilder)
        where T: class;
}