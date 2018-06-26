using System;
using System.Collections.Generic;

namespace CoreValidation.Validators
{
    public interface IValidatorsRepository
    {
        IReadOnlyCollection<Type> Types { get; }

        Validator<T> Get<T>()
            where T: class;
    }
}