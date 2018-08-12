using System.Linq;
using BenchmarkDotNet.Attributes;
using CoreValidation.Validators;

namespace CoreValidation.PerformanceTests.Messages
{
    [MemoryDiagnoser]
    public class Default
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
        public void Messages_Default()
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