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

            var errorMessages = this.Distinct();

            foreach (var errorMessage in errorMessages)
            {
                stringBuilder.AppendLine(errorMessage);
            }

            return stringBuilder.ToString();
        }
    }
}