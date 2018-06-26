using CoreValidation.Specifications;

namespace CoreValidation.Validators
{
    public delegate IMemberSpecification<TModel, TMember> MemberValidator<TModel, TMember>(IMemberSpecification<TModel, TMember> memberSpecification)
        where TModel : class;
}