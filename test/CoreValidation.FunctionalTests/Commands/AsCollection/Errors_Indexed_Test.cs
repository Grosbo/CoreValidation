using System.Collections.Generic;
using CoreValidation.Specifications;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CoreValidation.FunctionalTests.Commands.AsCollection
{
    // ReSharper disable once InconsistentNaming
    public class Errors_Indexed_Test
    {
        private class FavoritesNumbersModel
        {
            public IEnumerable<int> Numbers { get; set; }
        }

        [Fact]
        public void Errors_Indexed()
        {
            var favoritesNumbersModel = new FavoritesNumbersModel
            {
                Numbers = new[] {-100, 99, -10, 9, -1, 1, 0}
            };

            Specification<FavoritesNumbersModel> specification = s => s
                .Member(m => m.Numbers, m => m
                    .AsCollection(i => i.BetweenOrEqualTo(0, 99).WithMessage("Only 0-99 are accepted")
                    ));

            var result = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(favoritesNumbersModel);

            var expectedListReport = @"Numbers.0: Only 0-99 are accepted
Numbers.2: Only 0-99 are accepted
Numbers.4: Only 0-99 are accepted
";

            var expectedModelReportJson = @"
{
  'Numbers': {
    '0': [
      'Only 0-99 are accepted'
    ],
    '2': [
      'Only 0-99 are accepted'
    ],
    '4': [
      'Only 0-99 are accepted'
    ]
  }
}
";

            Assert.True(JToken.DeepEquals(JToken.FromObject(result.ToModelReport()), JToken.Parse(expectedModelReportJson)));
            Assert.Equal(expectedListReport, result.ToListReport().ToString());
        }
    }
}