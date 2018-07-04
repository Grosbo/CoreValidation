using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreValidation.Results.List
{
    public sealed class ListReport : List<string>
    {
        public override string ToString()
        {
            if (!this.Any())
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();

            foreach (var errorMessage in this)
            {
                stringBuilder.AppendLine(errorMessage);
            }

            return stringBuilder.ToString();
        }
    }
}