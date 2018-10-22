using System.Linq;
using BenchmarkDotNet.Attributes;
using CoreValidation.Errors.Args;
using CoreValidation.Factory.Specifications;
using CoreValidation.Results;
using CoreValidation.Specifications;

namespace CoreValidation.PerformanceTests
{
    [Config(typeof(CoreValidationDefaultConfig))]
    public class MessagesBenchmark
    {
        private IValidationContext _argsValidationContext;
        private IValidationContext _defaultsValidationContext;
        private IValidationContext _formatsValidationContext;
        private IValidationContext _messagesValidationContext;
        private Model[] _models;
        private IValidationContext _noErrorValidationContext;
        private IValidationResult<Model>[] _results;
        private IValidationContext _sameMessagesValidationContext;

        [Params(1, 10000, 100000)]
        public int N { get; set; }

        private void NoErrorSetup()
        {
            Specification<Model> specification = specs => specs
                .Member(m => m.Member, be => be
                    .Valid(v => true)
                    .Valid(v => true)
                    .Valid(v => true)
                    .Valid(v => true)
                    .Valid(v => true)
                    .Valid(v => true)
                    .Valid(v => true)
                    .Valid(v => true)
                    .Valid(v => true)
                    .Valid(v => true)
                );

            _noErrorValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void DefaultsSetup()
        {
            Specification<Model> specification = specs => specs
                .Member(m => m.Member, be => be
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                );

            _defaultsValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void SameMessagesSetup()
        {
            Specification<Model> specification = specs => specs
                .Member(m => m.Member, be => be
                    .Valid(v => false, "Message")
                    .Valid(v => false, "Message")
                    .Valid(v => false, "Message")
                    .Valid(v => false, "Message")
                    .Valid(v => false, "Message")
                    .Valid(v => false, "Message")
                    .Valid(v => false, "Message")
                    .Valid(v => false, "Message")
                    .Valid(v => false, "Message")
                    .Valid(v => false, "Message")
                );

            _sameMessagesValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void MessagesSetup()
        {
            Specification<Model> specification = specs => specs
                .Member(m => m.Member, be => be
                    .Valid(v => false, "Message 0")
                    .Valid(v => false, "Message 1")
                    .Valid(v => false, "Message 2")
                    .Valid(v => false, "Message 3")
                    .Valid(v => false, "Message 4")
                    .Valid(v => false, "Message 5")
                    .Valid(v => false, "Message 6")
                    .Valid(v => false, "Message 7")
                    .Valid(v => false, "Message 8")
                    .Valid(v => false, "Message 9")
                );

            _messagesValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void ArgsSetup()
        {
            Specification<Model> specification = specs => specs
                .Member(m => m.Member, be => be
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 0)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 1)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 2)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 3)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 4)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 5)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 6)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 7)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 8)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 9)})
                );

            _argsValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void FormatsSetup()
        {
            Specification<Model> specification = specs => specs
                .Member(m => m.Member, be => be
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 0)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 1)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 2)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 3)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 4)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 5)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 6)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 7)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 8)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), NumberArg.Create("id", 9)})
                );

            _formatsValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            NoErrorSetup();
            DefaultsSetup();
            SameMessagesSetup();
            MessagesSetup();
            ArgsSetup();
            FormatsSetup();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _models = Enumerable.Range(0, N).Select(n => new Model
            {
                Member = new object()
            }).ToArray();

            _results = new IValidationResult<Model>[N];
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
        public void NoError()
        {
            RunContext(_noErrorValidationContext);
        }

        [Benchmark]
        public void Defaults()
        {
            RunContext(_defaultsValidationContext);
        }

        [Benchmark]
        public void SameMessages()
        {
            RunContext(_sameMessagesValidationContext);
        }

        [Benchmark]
        public void Messages()
        {
            RunContext(_messagesValidationContext);
        }

        [Benchmark]
        public void Arguments()
        {
            RunContext(_argsValidationContext);
        }

        [Benchmark]
        public void Formats()
        {
            RunContext(_formatsValidationContext);
        }

        private class Model
        {
            public object Member { get; set; }
        }
    }
}