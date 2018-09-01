using System.Linq;
using BenchmarkDotNet.Attributes;
using CoreValidation.Errors.Args;
using CoreValidation.Factory.Specifications;
using CoreValidation.Specifications;

namespace CoreValidation.PerformanceTests
{
    [MemoryDiagnoser]
    public class MessagesBenchmark
    {
        private IValidationContext _argsValidationContext;

        private IValidationContext _defaultsValidationContext;
        private IValidationContext _formatsValidationContext;
        private IValidationContext _messagesValidationContext;
        private LoginModel[] _models;

        [Params(1, 500, 5000)]
        public int N { get; set; }

        private void DefaultsSetup()
        {
            Specification<LoginModel> specification = specs => specs
                .For(m => m.Email, be => be
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                )
                .For(m => m.Password, be => be
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                )
                .For(m => m.Nickname, be => be
                    .Valid(v => false)
                    .Valid(v => false)
                    .Valid(v => false)
                )
                .For(m => m.RememberMe, be => be
                    .Valid(rememberMe => false)
                )
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false)
                .Valid(m => false);

            _defaultsValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void MessagesSetup()
        {
            Specification<LoginModel> specification = specs => specs
                    .For(m => m.Email, be => be
                        .Valid(v => false, "Message 1")
                        .Valid(v => false, "Message 2")
                        .Valid(v => false, "Message 3")
                        .Valid(v => false, "Message 4")
                        .Valid(v => false, "Message 5")
                    )
                    .For(m => m.Password, be => be
                        .Valid(v => false, "Message 6")
                        .Valid(v => false, "Message 7")
                        .Valid(v => false, "Message 8")
                        .Valid(v => false, "Message 9")
                        .Valid(v => false, "Message 10")
                        .Valid(v => false, "Message 11")
                        .Valid(v => false, "Message 12")
                        .Valid(v => false, "Message 13")
                    )
                    .For(m => m.Nickname, be => be
                        .Valid(v => false, "Message 14")
                        .Valid(v => false, "Message 15")
                        .Valid(v => false, "Message 16")
                    )
                    .For(m => m.RememberMe, be => be
                        .Valid(rememberMe => false, "Message 17")
                    )
                    .Valid(m => false, "Message 18")
                    .Valid(m => false, "Message 19")
                    .Valid(m => false, "Message 20")
                    .Valid(m => false, "Message 21")
                    .Valid(m => false, "Message 22")
                ;

            _messagesValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void ArgsSetup()
        {
            Specification<LoginModel> specification = specs => specs
                .For(m => m.Email, be => be
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 1)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 2)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 3)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 4)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 5)})
                )
                .For(m => m.Password, be => be
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 6)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 7)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 8)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 9)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 10)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 11)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 12)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 13)})
                )
                .For(m => m.Nickname, be => be
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 14)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 15)})
                    .Valid(v => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 16)})
                )
                .For(m => m.RememberMe, be => be
                    .Valid(rememberMe => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 17)})
                )
                .Valid(m => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 18)})
                .Valid(m => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 19)})
                .Valid(m => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 20)})
                .Valid(m => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 21)})
                .Valid(m => false, "{title} {id}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 22)});

            _argsValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        private void FormatsSetup()
        {
            Specification<LoginModel> specification = specs => specs
                .For(m => m.Email, be => be
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 1)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 2)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 3)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 4)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 5)})
                )
                .For(m => m.Password, be => be
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 6)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 7)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 8)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 9)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 10)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 11)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 12)})
                    .Valid(v => false, "{title|case=lower} {id|format=000.0}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 13)})
                )
                .For(m => m.Nickname, be => be
                    .Valid(v => false, "{title|case=upper} {id|format=0.000}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 14)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.000}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 15)})
                    .Valid(v => false, "{title|case=upper} {id|format=0.000}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 16)})
                )
                .For(m => m.RememberMe, be => be
                    .Valid(rememberMe => false, "{title} {id|format=00}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 17)})
                )
                .Valid(m => false, "{title|case=lower} {id|format=000.000}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 18)})
                .Valid(m => false, "{title|case=lower} {id|format=000.000}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 19)})
                .Valid(m => false, "{title|case=lower} {id|format=000.000}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 20)})
                .Valid(m => false, "{title|case=lower} {id|format=000.000}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 21)})
                .Valid(m => false, "{title|case=lower} {id|format=000.000}", new IMessageArg[] {new TextArg("title", "Message"), new NumberArg("id", 22)});

            _formatsValidationContext = ValidationContext.Factory.Create(options => options.AddSpecification(specification));
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            DefaultsSetup();
            MessagesSetup();
            ArgsSetup();
            FormatsSetup();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _models = Enumerable.Range(0, N).Select(n => new LoginModel
            {
                Email = n.ToString(),
                Password = n.ToString(),
                Nickname = n.ToString(),
                RememberMe = false
            }).ToArray();
        }

        [Benchmark]
        public void Defaults()
        {
            for (var i = 0; i < N; ++i)
            {
                // ReSharper disable once UnusedVariable
                var result = _defaultsValidationContext.Validate(_models[i]);
            }
        }

        [Benchmark]
        public void Messages()
        {
            for (var i = 0; i < N; ++i)
            {
                // ReSharper disable once UnusedVariable
                var result = _messagesValidationContext.Validate(_models[i]);
            }
        }

        [Benchmark]
        public void Arguments()
        {
            for (var i = 0; i < N; ++i)
            {
                // ReSharper disable once UnusedVariable
                var result = _argsValidationContext.Validate(_models[i]);
            }
        }

        [Benchmark]
        public void Formats()
        {
            for (var i = 0; i < N; ++i)
            {
                // ReSharper disable once UnusedVariable
                var result = _formatsValidationContext.Validate(_models[i]);
            }
        }

        private class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Nickname { get; set; }
            public bool? RememberMe { get; set; }
        }
    }
}