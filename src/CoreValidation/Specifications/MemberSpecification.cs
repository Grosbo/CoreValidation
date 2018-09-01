namespace CoreValidation.Specifications
{
    public delegate IMemberSpecificationBuilder<TModel, TMember> MemberSpecification<TModel, TMember>(IMemberSpecificationBuilder<TModel, TMember> memberSpecificationBuilder)
        where TModel : class;
}