using System.Collections.Generic;
using CoreValidation.Specifications;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CoreValidation.FunctionalTests.Commands.AsCollection
{
    // ReSharper disable once InconsistentNaming
    public class Collection_Of_Models_Test
    {
        private class RoomModel
        {
            public IEnumerable<PersonModel> Persons { get; set; }
        }

        private class PersonModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [Fact]
        public void Collection_Of_Models()
        {
            var roomModel = new RoomModel
            {
                Persons = new[]
                {
                    new PersonModel {Name = "Jimmie", Age = 33},
                    new PersonModel {Name = "Monica", Age = 24},
                    new PersonModel {Name = "M", Age = 11},
                    new PersonModel {Name = "Martha", Age = -23},
                    new PersonModel {Name = "Ela", Age = 3},
                    new PersonModel {Age = 1233}
                }
            };

            Specification<PersonModel> personSpecification = s => s
                .Member(m => m.Name, m => m.MinLength(2).WithMessage("Name is too short"))
                .Member(m => m.Age, m => m.GreaterOrEqualTo(0))
                .Valid(m => (m.Name != null) && (m.Age < 1000)).WithMessage("Nameless immortals are invalid");

            Specification<RoomModel> roomSpecification = s => s
                .Member(m => m.Persons, m => m.AsCollection(i => i.AsModel(personSpecification)));

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(roomSpecification)
            );

            var result = validationContext.Validate(roomModel);

            var expectedListReport = @"Persons.2.Name: Name is too short
Persons.3.Age: Number should be greater than (or equal to) 0
Persons.5: Nameless immortals are invalid
Persons.5.Name: Required
";

            var expectedModelReportJson = @"
{
  'Persons': {
    '2': {
        'Name': [
          'Name is too short'
        ]
    },
    '3': {
        'Age': [
          'Number should be greater than (or equal to) 0'
        ]
    },
    '5': {
        'Name': [
          'Required'
        ],
        '': [
          'Nameless immortals are invalid'
        ]
    }
  }
}
";
            Assert.True(JToken.DeepEquals(JToken.FromObject(result.ToModelReport()), JToken.Parse(expectedModelReportJson)));
            Assert.Equal(expectedListReport, result.ToListReport().ToString());

            Specification<RoomModel> roomSpecificationRepo = s => s
                .Member(m => m.Persons, m => m.AsCollection(i => i.AsModel()));

            var resultUsingRepository = ValidationContext.Factory.Create(options => options
                    .AddSpecification(roomSpecificationRepo)
                    .AddSpecification(personSpecification)
                )
                .Validate(roomModel);

            Assert.True(JToken.DeepEquals(JToken.FromObject(resultUsingRepository.ToModelReport()), JToken.Parse(expectedModelReportJson)));
            Assert.Equal(expectedListReport, resultUsingRepository.ToListReport().ToString());
        }
    }
}