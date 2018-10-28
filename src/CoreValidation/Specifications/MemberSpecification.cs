namespace CoreValidation.Specifications
{
    /// <summary>
    /// Specification (Fluent API) for the member (of type <typeparamref name="TMember"/>) belonging to the model (of type <typeparamref name="TModel"/>).
    /// </summary>
    /// <typeparam name="TModel">Type of the model that the member belongs to.</typeparam>
    /// <typeparam name="TMember">Type of the member to be specified.</typeparam>
    /// <param name="memberSpecificationBuilder">Builder with all of the Fluent API extensions to build the specification with.</param>
    public delegate IMemberSpecificationBuilder<TModel, TMember> MemberSpecification<TModel, TMember>(IMemberSpecificationBuilder<TModel, TMember> memberSpecificationBuilder)
        where TModel : class;
}