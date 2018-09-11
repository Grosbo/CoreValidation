using System.Collections.Generic;

namespace CoreValidation.Results.Model
{
    public sealed class ModelReport : Dictionary<string, IModelReport>, IModelReport
    {
        internal static ModelReport Empty { get; } = new ModelReport();
    }
}