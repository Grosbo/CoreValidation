using System.Linq;
using BenchmarkDotNet.Attributes;
using CoreValidation.Validators;

namespace CoreValidation.PerformanceTests.Messages
{
    [MemoryDiagnoser]
    public class WithoutArgs
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
        public void Messages_WithoutArgs()
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