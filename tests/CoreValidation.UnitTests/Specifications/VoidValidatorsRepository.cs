using System;
using System.Collections.Generic;
using CoreValidation.Validators;

namespace CoreValidation.UnitTests.Specifications
{
    public class VoidValidatorsRepository : IValidatorsRepository
    {
        public IReadOnlyCollection<Type> Types => throw new NotImplementedException();

        public Validator<T> Get<T>()
            where T : class
        {
            throw new NotImplementedException();
        }
    }
}