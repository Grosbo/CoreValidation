using CoreValidation.Factory.Specifications;
using CoreValidation.WorkloadTests.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreValidation.WorkloadTests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private static readonly IValidationContext _loginModelValidationContext = ValidationContext.Factory.Create(options => options
            .AddSpecification<LoginModel>(login => login
                .Member(m => m.Email, be => be
                    .Email()
                    .MaxLength(50)
                    .Valid(email => email.EndsWith("gmail.com"), "Only gmails are accepted"))
                .Member(m => m.Password, be => be.NotEmpty()))
        );

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            var validationResult = _loginModelValidationContext.Validate(loginModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToModelReport());
            }

            return Ok();
        }

        [HttpPost("void")]
        public IActionResult SignUpVoid(LoginModel loginModel)
        {
            return Ok(loginModel);
        }
    }
}