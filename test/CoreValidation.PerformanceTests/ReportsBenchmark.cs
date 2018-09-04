using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CoreValidation.Factory.Specifications;
using CoreValidation.Results;
using CoreValidation.Specifications;

namespace CoreValidation.PerformanceTests
{
    [Config(typeof(CoreValidationDefaultConfig))]
    public class ReportsBenchmark
    {
        private object[] _reports;
        private IValidationResult<MasterModel>[] _results;

        [Params(1, 10000, 100000)]
        public int N { get; set; }

        [IterationSetup]
        public void IterationSetup()
        {
            Specification<MasterModel> specification = specs => specs
                .Valid(m => false, "Self 0")
                .Valid(m => false, "Self 1")
                .Valid(m => false, "Self 2")
                .Valid(m => false, "Self 3")
                .Valid(m => false, "Self 4")
                .Valid(m => false, "Self 5")
                .Valid(m => false, "Self 6")
                .Valid(m => false, "Self 7")
                .Valid(m => false, "Self 8")
                .Valid(m => false, "Self 9")
                .For(m => m.Member, be => be
                    .Valid(m => false, "Valid 0")
                    .Valid(m => false, "Valid 1")
                    .Valid(m => false, "Valid 2")
                    .Valid(m => false, "Valid 3")
                    .Valid(m => false, "Valid 4")
                    .Valid(m => false, "Valid 5")
                    .Valid(m => false, "Valid 6")
                    .Valid(m => false, "Valid 7")
                    .Valid(m => false, "Valid 8")
                    .Valid(m => false, "Valid 9"))
                .For(m => m.Member, be => be
                    .ValidRelative(m => false, "Relative 0")
                    .ValidRelative(m => false, "Relative 1")
                    .ValidRelative(m => false, "Relative 2")
                    .ValidRelative(m => false, "Relative 3")
                    .ValidRelative(m => false, "Relative 4")
                    .ValidRelative(m => false, "Relative 5")
                    .ValidRelative(m => false, "Relative 6")
                    .ValidRelative(m => false, "Relative 7")
                    .ValidRelative(m => false, "Relative 8")
                    .ValidRelative(m => false, "Relative 9"))
                .For(m => m.NullableMember, be => be.ValidNullable(m => m
                    .Valid(m1 => false, "Nullable 0")
                    .Valid(m1 => false, "Nullable 1")
                    .Valid(m1 => false, "Nullable 2")
                    .Valid(m1 => false, "Nullable 3")
                    .Valid(m1 => false, "Nullable 4")
                    .Valid(m1 => false, "Nullable 5")
                    .Valid(m1 => false, "Nullable 6")
                    .Valid(m1 => false, "Nullable 7")
                    .Valid(m1 => false, "Nullable 8")
                    .Valid(m1 => false, "Nullable 9")))
                .For(m => m.ModelMember, be => be.ValidModel(m => m
                    .Valid(m1 => false, "Model 0")
                    .Valid(m1 => false, "Model 1")
                    .Valid(m1 => false, "Model 2")
                    .Valid(m1 => false, "Model 3")
                    .Valid(m1 => false, "Model 4")
                    .Valid(m1 => false, "Model 5")
                    .Valid(m1 => false, "Model 6")
                    .Valid(m1 => false, "Model 7")
                    .Valid(m1 => false, "Model 8")
                    .Valid(m1 => false, "Model 9")))
                .For(m => m.Collection, be => be.ValidCollection(m => m
                    .Valid(m1 => false, "Collection 0")
                    .Valid(m1 => false, "Collection 1")))
                .For(m => m.CollectionOfModels, be => be.ValidModelsCollection(m => m
                    .Valid(m1 => false, "Collection models 0")
                    .Valid(m1 => false, "Collection models 1")));

            var validationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));

            var models = Enumerable.Range(0, N).Select(n => new MasterModel
            {
                Member = new object(),
                NullableMember = false,
                ModelMember = new NestedModel
                {
                    NestedMember = new object()
                },
                Collection = new[]
                {
                    new object(),
                    new object(),
                    new object(),
                    new object(),
                    new object()
                },
                CollectionOfModels = new[]
                {
                    new NestedModel {NestedMember = new object()},
                    new NestedModel {NestedMember = new object()},
                    new NestedModel {NestedMember = new object()},
                    new NestedModel {NestedMember = new object()},
                    new NestedModel {NestedMember = new object()}
                }
            }).ToArray();

            _results = new IValidationResult<MasterModel>[N];

            for (var i = 0; i < N; ++i)
            {
                _results[i] = validationContext.Validate(models[i]);
            }

            _reports = new object[N];
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            _results = null;
            _reports = null;
        }


        [Benchmark]
        public void ListReport()
        {
            for (var i = 0; i < N; ++i)
            {
                _reports[i] = _results[i].ToListReport();
            }
        }

        [Benchmark]
        public void ModelReport()
        {
            for (var i = 0; i < N; ++i)
            {
                _reports[i] = _results[i].ToModelReport();
            }
        }

        private class MasterModel
        {
            public object Member { get; set; }
            public bool? NullableMember { get; set; }
            public NestedModel ModelMember { get; set; }
            public IEnumerable<object> Collection { get; set; }
            public IEnumerable<NestedModel> CollectionOfModels { get; set; }
        }

        private class NestedModel
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public object NestedMember { get; set; }
        }
    }
}