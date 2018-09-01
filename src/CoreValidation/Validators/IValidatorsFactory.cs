using System.Collections.Generic;
using CoreValidation.Specifications;

namespace CoreValidation.Validators
{
    internal interface IValidatorsFactory
    {
        IEnumerable<string> Keys { get; }
        IValidator<T> Get<T>(string key = null) where T : class;
        IValidator<T> GetOrInit<T>(Specification<T> specification = null, string key = null) where T : class;
    }
}