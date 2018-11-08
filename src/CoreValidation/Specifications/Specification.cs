namespace CoreValidation.Specifications
{
    /// <summary>
    ///     Specification (Fluent API) for the models of type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">Specified type</typeparam>
    /// <param name="specificationBuilder">Builder with all of the Fluent API extensions to build the specification with</param>
    public delegate ISpecificationBuilder<T> Specification<T>(ISpecificationBuilder<T> specificationBuilder)
        where T : class;
}