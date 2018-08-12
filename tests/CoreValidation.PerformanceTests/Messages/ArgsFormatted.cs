using System.Linq;
using BenchmarkDotNet.Attributes;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Validators;

namespace CoreValidation.PerformanceTests.Messages
{
    [MemoryDiagnoser]
    public class ArgsFormatted
    {
        private LoginModel[] _models;
        private IValidationContext _validationContext;

        [Params(1, 500, 5000)]
        public int N { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Validator<LoginModel> loginValidator = login => login
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

            _validationContext = ValidationContext.Factory.Create(options => options.AddValidator(loginValidator));
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
        public void Messages_ArgsFormatted()
        {
            for (var i = 0; i < N; ++i)
            {
                // ReSharper disable once UnusedVariable
                var result = _validationContext.Validate(_models[i]);
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