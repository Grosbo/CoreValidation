using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CoreValidation.Factory.Specifications;
using CoreValidation.Results;
using CoreValidation.Specifications;

namespace CoreValidation.PerformanceTests
{
    [Config(typeof(CoreValidationDefaultConfig))]
    public class RulesBenchmark
    {
        private IValidationContext _collectionValidationContext;
        private IValidationContext _memberValidationContext;
        private MasterModel[] _models;
        private IValidationContext _modelsCollectionValidationContext;
        private IValidationContext _modelValidationContext;
        private IValidationContext _nullableValidationContext;
        private IValidationContext _relationValidationContext;

        private IValidationResult<MasterModel>[] _results;

        private IValidationContext _selfValidationContext;

        [Params(1, 10000, 100000)]
        public int N { get; set; }

        private void SelfSetup()
        {
            Specification<MasterModel> specification = specs => specs
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false);

            _selfValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void MemberSetup()
        {
            Specification<MasterModel> specification = specs => specs
                    .Member(m => m.Member, be => be
                        .Valid(m => false, "error 0")
                        .Valid(m => false, "error 1")
                        .Valid(m => false, "error 2")
                        .Valid(m => false, "error 3")
                        .Valid(m => false, "error 4")
                        .Valid(m => false, "error 5")
                        .Valid(m => false, "error 6")
                        .Valid(m => false, "error 7")
                        .Valid(m => false, "error 8")
                        .Valid(m => false, "error 9")
                    )
                ;

            _memberValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void RelationSetup()
        {
            Specification<MasterModel> specification = specs => specs
                .Member(m => m.Member, be => be
                    .AsRelative(m => false)
                    .AsRelative(m => false)
                    .AsRelative(m => false)
                    .AsRelative(m => false)
                    .AsRelative(m => false)
                    .AsRelative(m => false)
                    .AsRelative(m => false)
                    .AsRelative(m => false)
                    .AsRelative(m => false)
                    .AsRelative(m => false)
                );

            _relationValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void NullableSetup()
        {
            Specification<MasterModel> specification = specs => specs
                .Member(m => m.NullableMember, be => be.AsNullable(m => m
                        .Valid(m1 => false, "error 0")
                        .Valid(m1 => false, "error 1")
                        .Valid(m1 => false, "error 2")
                        .Valid(m1 => false, "error 3")
                        .Valid(m1 => false, "error 4")
                        .Valid(m1 => false, "error 5")
                        .Valid(m1 => false, "error 6")
                        .Valid(m1 => false, "error 7")
                        .Valid(m1 => false, "error 8")
                        .Valid(m1 => false, "error 9")
                    )
                );

            _nullableValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void ModelSetup()
        {
            Specification<MasterModel> specification = specs => specs
                .Member(m => m.ModelMember, be => be.AsModel(m => m
                    .Valid(m1 => false)
                    .Valid(m1 => false)
                    .Valid(m1 => false)
                    .Valid(m1 => false)
                    .Valid(m1 => false)
                    .Valid(m1 => false)
                    .Valid(m1 => false)
                    .Valid(m1 => false)
                    .Valid(m1 => false)
                    .Valid(m1 => false)
                ));

            _modelValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void CollectionSetup()
        {
            Specification<MasterModel> specification = specs => specs
                .Member(m => m.Collection, be => be.AsCollection(m => m
                    .Valid(m1 => false, "error 0")
                    .Valid(m1 => false, "error 1")
                ));

            _collectionValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void ModelsCollectionSetup()
        {
            Specification<MasterModel> specification = specs => specs
                .Member(m => m.CollectionOfModels, be => be.AsModelsCollection(m => m
                    .Valid(m1 => false)
                    .Valid(m1 => false)
                ));

            _modelsCollectionValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            SelfSetup();
            MemberSetup();
            RelationSetup();
            NullableSetup();
            ModelSetup();
            CollectionSetup();
            ModelsCollectionSetup();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _models = Enumerable.Range(0, N).Select(n => new MasterModel
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
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            _models = null;
            _results = null;
        }

        private void RunContext(IValidationContext context)
        {
            for (var i = 0; i < N; ++i)
            {
                _results[i] = context.Validate(_models[i]);
            }
        }

        [Benchmark]
        public void Self()
        {
            RunContext(_selfValidationContext);
        }

        [Benchmark]
        public void Member()
        {
            RunContext(_memberValidationContext);
        }

        [Benchmark]
        public void Relation()
        {
            RunContext(_relationValidationContext);
        }

        [Benchmark]
        public void Nullable()
        {
            RunContext(_nullableValidationContext);
        }

        [Benchmark]
        public void Model()
        {
            RunContext(_modelValidationContext);
        }

        [Benchmark]
        public void Collections()
        {
            RunContext(_collectionValidationContext);
        }

        [Benchmark]
        public void ModelsCollection()
        {
            RunContext(_modelsCollectionValidationContext);
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