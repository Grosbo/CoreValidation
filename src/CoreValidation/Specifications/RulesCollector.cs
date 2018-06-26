using System;
using CoreValidation.Exceptions;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal class RulesCollector
    {
        public IRulesCollection GetMemberRules<TModel, TMember>(MemberValidator<TModel, TMember> memberValidator)
            where TModel : class
        {
            if (memberValidator == null)
            {
                throw new ArgumentNullException(nameof(memberValidator));
            }

            var newSpecification = new MemberSpecification<TModel, TMember>();

            var processedSpecification = memberValidator(newSpecification);

            if (!ReferenceEquals(newSpecification, processedSpecification))
            {
                throw new InvalidProcessedReferenceException(typeof(MemberSpecification<TModel, TMember>));
            }

            return processedSpecification as MemberSpecification<TModel, TMember>;
        }
    }
}