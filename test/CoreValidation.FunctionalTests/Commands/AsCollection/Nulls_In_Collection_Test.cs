using System.Collections.Generic;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.FunctionalTests.Commands.AsCollection
{
    // ReSharper disable once InconsistentNaming
    public class Nulls_In_Collection_Test
    {
        private class FavoritesTagsModel
        {
            public IEnumerable<string> Tags { get; set; }
        }

        [Fact]
        public void Nulls_In_Collection()
        {
            var tagsModel = new FavoritesTagsModel
            {
                Tags = new[] {"sport", null, null, "books", "#", "cars"}
            };

            Specification<FavoritesTagsModel> specification = s => s
                .Member(m => m.Tags, m => m
                    .AsCollection(i => i
                        .MinLength(3).WithMessage("Tag should heave at least {min} characters")
                    ));

            var listReport = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(tagsModel)
                .ToListReport();

            var expectedListReport = @"Tags.1: Required
Tags.2: Required
Tags.4: Tag should heave at least 3 characters
";

            Assert.Equal(expectedListReport, listReport.ToString());

            Specification<FavoritesTagsModel> specificationAllowingNulls = s => s
                .Member(m => m.Tags, m => m
                    .AsCollection(i => i
                        .SetOptional()
                        .MinLength(3).WithMessage("Tag should heave at least {min} characters")
                    ));

            var listReportAllowingNulls = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specificationAllowingNulls)
                )
                .Validate(tagsModel)
                .ToListReport();

            var expectedReportAllowingNulls = @"Tags.4: Tag should heave at least 3 characters
";

            Assert.Equal(expectedReportAllowingNulls, listReportAllowingNulls.ToString());
        }
    }
}