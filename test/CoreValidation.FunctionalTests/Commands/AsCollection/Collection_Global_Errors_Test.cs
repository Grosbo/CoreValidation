using System.Collections.Generic;
using System.Linq;
using CoreValidation.Specifications;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CoreValidation.FunctionalTests.Commands.AsCollection
{
    // ReSharper disable once InconsistentNaming
    public class Collection_Global_Errors_Test
    {
        private class ConsentsModel
        {
            public IEnumerable<bool?> Consents { get; set; }
        }

        [Fact]
        public void Collection_General_Errors()
        {
            var consentsModel = new ConsentsModel
            {
                Consents = new bool?[] {true, true, null, false, true}
            };

            Specification<ConsentsModel> specification = s => s
                .Member(m => m.Consents, m => m
                    .NotEmptyCollection()
                    .MaxCollectionSize(4)
                    .Valid(c => !c.Contains(null)).WithMessage("Missing decisions!")
                    .AsCollection(i => i.True().WithMessage("Consent must be given!"))
                );

            var result = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(consentsModel);

            var expectedListReport = @"Consents: Collection should have maximum 4 elements
Consents: Missing decisions!
Consents.2: Required
Consents.3: Consent must be given!
";

            var expectedModelReportJson = @"
{
  'Consents': {
    '2': [
          'Required'
    ],
    '3': [
          'Consent must be given!'
    ],
    '' : [
        'Collection should have maximum 4 elements',
        'Missing decisions!'
    ]
  }
}
";

            Assert.True(JToken.DeepEquals(JToken.FromObject(result.ToModelReport()), JToken.Parse(expectedModelReportJson)));
            Assert.Equal(expectedListReport, result.ToListReport().ToString());
        }
    }
}