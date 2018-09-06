using CoreValidation.Factory.Specifications;
using CoreValidation.WorkloadTests.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreValidation.WorkloadTests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private static readonly SignUpModelSpecification _signUpModelSpecification = new SignUpModelSpecification();

        private static readonly IValidationContext _signUpValidationContext = ValidationContext.Factory.Create(options => options
            .AddSpecificationsFromHolder(_signUpModelSpecification)
            .AddTranslations(_signUpModelSpecification.TranslationsPackage)
        );

        [HttpPost]
        public IActionResult SignUp(SignUpModel signUpModel)
        {
            var validationResult = _signUpValidationContext.Validate(signUpModel);

            if (!validationResult.IsValid())
            {
                return BadRequest(validationResult.ToModelReport());
            }

            return Ok();
        }

        [HttpPost("void")]
        public IActionResult SignUpVoid(SignUpModel signUpModel)
        {
            return Ok(signUpModel);
        }
    }
}