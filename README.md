# CoreValidation

![CoreValidation logo](https://github.com/bartoszlenar/CoreValidation/raw/master/logo/logo_128.png)

[![Build status](https://ci.appveyor.com/api/projects/status/y9itfpoy8su7yuxk?svg=true)](https://ci.appveyor.com/project/bartoszlenar/corevalidation) [![NuGet package](https://img.shields.io/nuget/v/CoreValidation.svg)](https://www.nuget.org/packages/CoreValidation)

## About

**Advanced model validation for the modern dotnet world** - yes, this is the tagline, but what does the library really do?

* Easly handles advanced validation scenarios for complex models within your app
* Delivers friendly fluent api to validate reference and value types, nested models, nullables, collections and relations between all of them
* Only 67KB, based on .NET Standard 2.0, no extra dependencies
* Designed to work well with DDD\CQRS approaches
* Supports variety of report types and translations out of the box
* Extensibility points at every level for advanced behavior customization

## Show me the code

The code is worth a thousand words - so below you will find complete end-to-end example presenting most of the main features

### Know your model

Let's assume there is `UserModel` for creating a new user and we're going to validate

* Members (`Email`, `Password`, etc.)
* Relation between them (both `Password` and `PasswordConfirmation` need to be equal)
* Nested model (`Address`) with its own set of rules
* Collection (`Tags`)
* Nullable (`Age`)

``` csharp
class UserModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
    public AddressModel Address { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public int? Age { get; set; }
}
```

``` csharp
class AddressModel
{
    public string Street { get; set; }
    public string PostCode { get; set; }
    public int CountryId { get; set; }
}
```

### Get it

The package is hosted on [nuget.org](https://www.nuget.org/packages/CoreValidation)

``` bash
dotnet add package CoreValidation --version 1.0.0-beta
```

### Define your validators

You can create them inline or in separate classes - it's up to you how you would like to structure your code

``` csharp
Validator<AddressModel> addressValidator = specs => specs
    // Validate model's members:
    .For(m => m.Street, be => be.NotWhiteSpace())
    .For(m => m.PostCode, be => be.MaxLength(10))
    .For(m => m.CountryId, be => be.GreaterThan(0))

    // Validate model in it's global scope:
    .Valid(m => (m.Street != null) &&
                (m.PostCode != null) &&
                !m.Street.Contains(m.Street),
        "Both street and postcode are required and need to be put separate");
```

``` csharp
Validator<UserModel> userValidator = specs => specs
    // Validate members with predefined rules:
    .For(m => m.Email, be => be.Email())

    // Apply many rules along with custom predicates:
    .For(m => m.Password, be => be
        .MinLength(6)
        .NotWhiteSpace()
        .Valid(v => v.Any(char.IsUpper), "Must contain an upper case letter")
        .Valid(v => v.Any(char.IsDigit), "Must contain a digit"))

    // Validate relations with other members:
    .For(m => m.PasswordConfirmation, be => be
        .ValidRelative(m => m.Password == m.PasswordConfirmation,
            "Confirmation doesn't match password"))

    // Validate nested models (using another validator):
    .For(m => m.Address, be => be.ValidModel(addressValidator))

    // Validate collections:
    .For(m => m.Tags, be => be
        // Override default message for predefine rule:
        .NotEmpty(message: "At least one tag required")
        .MaxSize(10, message: "Max 10 tags allowed")
        // Validate every item inside of the collection:
        .ValidCollection(i => i
            .NotWhiteSpace()
            .MaxLength(10)
            .Valid(v => char.IsLetter(v.FirstOrDefault()), "Tag must start with a letter")
            .Valid(v => v.All(char.IsLetterOrDigit), "Tag can contains only letters and digits")))

    // Validate nullables:
    .For(m => m.Age, be => be
        // Mark member as not required:
        .Optional()
        // If present, apply some rules:
        .GreaterThan(0));
```

### Create validation context

Use a factory to create and configure `IValidationContext` instance, so you can keep it e.g. within your bounded context or CQRS feature

``` csharp
var validationContext = ValidationContext.Factory.Create(options => options
    // Add validators
    .AddValidator(userValidator)
    .AddValidator(addressValidator)

    // Set additional options (there are lot of them available...)
    .SetValidationStategy(ValidationStrategy.Complete)
    .SetRequiredError("Value is required")
);
```

### Validate the model

When the time comes, use `IValidationContext` instance to validate incoming model

``` csharp
var result = validationContext.Validate(userModel);
```

### Process the result

Once the result is produced, you can process it in many different ways, e.g. convert it to `ModelReport`

``` csharp
var modelReport = result.ToModelReport();
```

Assuming the validated model is like

``` csharp
var userModel = new UserModel
{
    Email = "invalid@@@email.com",
    Password = "12345",
    PasswordConfirmation = "1234567",
    Address = new AddressModel()
    {
        Street = null,
        PostCode = "1234-5678-90",
        CountryId = -1
    },
    Tags = new[]
    {
        "Test",
        "special#@",
        "test2",
        null,
        "1stdigit",
    },
    Age = null
};
```

The outcome of `JsonConvert.SerializeObject(modelReport)` would be

``` json
{
    "Email": ["Text value should be a valid email"],
    "Password": ["Text value should have minimum 6 characters", "Must contain an upper case letter"],
    "PasswordConfirmation": ["Confirmation doesn't match password"],
    "Address": {
        "": ["Both street and postcode are required and need to be put separate"],
        "Street": ["Value is required"],
        "PostCode": ["Text value should have maximum 10 characters"],
        "CountryId": ["Number should be greater than 0"]
    },
    "Tags": {
        "1": ["Tag can contains only letters and digits"],
        "3": ["Value is required"],
        "4": ["Tag must start with a letter"]
    }
}
```

Variety of other actions are available, like `ListReport`, which is a collection of strings

``` csharp
var listReport = result.ToListReport();
```

You can enumerate over `listReport` to read the error messages one by one or combine them all using  `listReport.ToString()`

```
Email: Text value should be a valid email
Password: Text value should have minimum 6 characters
Password: Must contain an upper case letter
PasswordConfirmation: Confirmation doesn't match password
Address: Both street and postcode are required and need to put separate
Address.Street: Value is required
Address.PostCode: Text value should have maximum 10 characters
Address.CountryId: Number should be greater than 0
Tags.1: Tag can contains only letters and digits
Tags.3: Value is required
Tags.4: Tag must start with a letter
```

Generating report is, of course, not required at all

``` csharp
// No report, just information about the model:
if (result.IsValid())
{
  // do cool stuff
}
```

``` csharp
// Throws exception if model is invalid:
result.ThrowIfInvalid();
```

Want to see this example in action and debug it for yourself? Go ahead, it exists as a [functional test](tests/CoreValidation.FunctionalTests/ShowMeTheCode/ShowMeTheCodeTests.cs).