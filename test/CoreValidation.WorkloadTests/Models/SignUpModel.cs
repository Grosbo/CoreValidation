using System;
using System.Collections.Generic;

namespace CoreValidation.WorkloadTests.Models
{
    public class SignUpModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public AddressModel Address { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}