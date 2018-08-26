using CoreValidation.Specifications;

namespace CoreValidation.Validators
{
    public delegate IMemberSpecificationBuilder<TModel, TMember> MemberValidator<TModel, TMember>(IMemberSpecificationBuilder<TModel, TMember> memberSpecificationBuilder)
        where TModel : class;
}