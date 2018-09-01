﻿using System.Linq;
using BenchmarkDotNet.Attributes;
using CoreValidation.Factory.Specifications;
using CoreValidation.Specifications;

namespace CoreValidation.PerformanceTests.Readme
{
    [MemoryDiagnoser]
    public class QuickstartBenchmark
    {
        private LoginModel[] _models;
        private IValidationContext _validationContext;

        [Params(1, 500, 5000)]
        public int N { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Specification<LoginModel> loginSpecification = login => login
                .For(m => m.Email, be => be
                    .Email()
                    .MaxLength(50)
                    .Valid(email => email.EndsWith("gmail.com"), "Only gmails are accepted"))
                .For(m => m.Password, be => be.NotEmpty());

            _validationContext = ValidationContext.Factory.Create(options => options.AddSpecification(loginSpecification));
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _models = Enumerable.Range(0, N).Select(n => new LoginModel
            {
                Email = $"invalidemail_{n}",
                Password = ""
            }).ToArray();
        }

        [Benchmark]
        public void Quickstart()
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
        }
    }
}