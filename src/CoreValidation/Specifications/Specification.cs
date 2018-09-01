namespace CoreValidation.Specifications
{
    public delegate ISpecificationBuilder<T> Specification<T>(ISpecificationBuilder<T> specificationBuilder)
        where T : class;
}