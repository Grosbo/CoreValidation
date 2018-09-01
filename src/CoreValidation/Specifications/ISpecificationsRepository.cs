using System;
using System.Collections.Generic;

namespace CoreValidation.Specifications
{
    public interface ISpecificationsRepository
    {
        IReadOnlyCollection<Type> Types { get; }

        Specification<T> Get<T>()
            where T : class;
    }
}