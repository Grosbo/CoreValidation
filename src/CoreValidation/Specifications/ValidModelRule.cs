using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Options;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal abstract class ValidModelRule
    {
    }

    internal sealed class ValidModelRule<TMember> : ValidModelRule, IRule
        where TMember : class
    {
        public ValidModelRule(Validator<TMember> validator = null)
        {
            Validator = validator;
        }

        public Validator<TMember> Validator { get; }

        public ErrorsCollection Compile(object[] args)
        {
            return Compile(
                Validator,
                (TMember)args[0],
                (IValidatorsRepository)args[1],
                (ValidationStrategy)args[2],
                (int)args[3],
                (IRulesOptions)args[4]
            );
        }

        public static ErrorsCollection Compile(Validator<TMember> validator, TMember memberValue, IValidatorsRepository validatorsRepository, ValidationStrategy validationStrategy, int depth, IRulesOptions rulesOptions)
        {
            var finalValidator = validator ?? validatorsRepository.Get<TMember>();

            return Compile(memberValue, finalValidator, validatorsRepository, validationStrategy, depth, rulesOptions);
        }

        private static ErrorsCollection Compile<T>(T model, Validator<T> validator, IValidatorsRepository validatorsRepository, ValidationStrategy validationStrategy, int depth, IRulesOptions rulesOptions)
            where T : class
        {
            var validatorToCompile = validator ?? validatorsRepository.Get<T>();

            var builder = new Specification<T>(model, validatorsRepository, validationStrategy, depth + 1, rulesOptions);

            var processedBuilder = validatorToCompile(builder) as Specification<T>;

            if (!ReferenceEquals(builder, processedBuilder))
            {
                throw new InvalidProcessedReferenceException(typeof(Specification<T>));
            }

            return builder.GetErrors();
        }
    }
}