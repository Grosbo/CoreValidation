using System.Collections.Generic;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal interface ISpecificationsRepository
    {
        IEnumerable<string> Keys { get; }
        ISpecification<T> Get<T>(string key = null) where T : class;
        ISpecification<T> GetOrInit<T>(Validator<T> validator = null, string key = null) where T : class;
    }
}