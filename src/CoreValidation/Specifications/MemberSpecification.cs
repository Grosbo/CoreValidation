namespace CoreValidation.Specifications
{
    /// <summary>
    /// Member specification is the fluent-api function that describes the valid state of the model's member.
    /// The commands collection that it contains is called member scope.
    /// </summary>
    /// <typeparam name="TModel">Type of the model that the member belongs to.</typeparam>
    /// <typeparam name="TMember">Type of the member to be specified.</typeparam>
    /// <param name="memberSpecificationBuilder">Builder with all of the fluent-api extensions to build the specification with.</param>
    public delegate IMemberSpecificationBuilder<TModel, TMember> MemberSpecification<TModel, TMember>(IMemberSpecificationBuilder<TModel, TMember> memberSpecificationBuilder)
        where TModel : class;
}