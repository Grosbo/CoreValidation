# CoreValidation

![CoreValidation logo](https://github.com/bartoszlenar/CoreValidation/raw/master/logo/logo_128.png)

[![Build status](https://ci.appveyor.com/api/projects/status/y9itfpoy8su7yuxk?svg=true)](https://ci.appveyor.com/project/bartoszlenar/corevalidation) [![NuGet package](https://img.shields.io/nuget/v/CoreValidation.svg)](https://www.nuget.org/packages/CoreValidation)

## About

**Advanced model validation for the modern dotnet world** - indeed, this is the tagline, but what does the library really do?

* Easily handles advanced validation scenarios for complex models within your app
* Delivers friendly fluent api to validate reference and value types, nested models, nullables, collections and relations between all of them
* Only 67KB, based on .NET Standard 2.0, no extra dependencies
* Designed to work well with DDD\CQRS approaches
* Supports variety of report types and translations out of the box
* Extensibility points at every level for advanced behavior customization

## Quickstart

The code is worth a thousand words - so below you will find complete end-to-end, very detailed example presenting most of the main features

### Know your model

Let's have a `UserModel` class for creating a new user in the app. and we're going to validate

* Members (`Email`, `Password`, etc.)
* Optional members (`Name`)
* Relation between them (both `Password` and `PasswordConfirmation` must match each other)
* Nested model (`Address`) with its own set of rules
* Collection (`Tags`)
* Nullable (`DateOfBirth`)

``` csharp
class UserModel
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
    public AddressModel Address { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public DateTime? DateOfBirth { get; set; }
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

### Install it

The package is hosted on [nuget.org](https://www.nuget.org/packages/CoreValidation), so to add it to your project, by default this is good enough:

``` bash
dotnet add package CoreValidation --version 1.0.0-beta
```

The second option is adding reference to the sources and it's not as painfull as you might think. Only .NET Standard 2.0, no additional dependencies, single project. Here:

``` bash
git clone --branch 1.0.0-beta git@github.com:bartoszlenar/CoreValidation.git
```

And you're interested in the content of `/src/CoreValidation` directory (and `CoreValidation.csproj` within it).

### Define your validators

You can create them inline or in separate classes - it's up to you how you would like to structure your code

``` csharp
Validator<AddressModel> addressValidator = specs => specs
    // Validate members in their scope (error attached to the selected member):
    .For(m => m.Street, be => be.NotWhiteSpace())
    .For(m => m.PostCode, be => be.MaxLength(10))
    .For(m => m.CountryId, be => be.GreaterThan(0))

    // Validate model in its global scope (error attached to the model instead of its member):
    .Valid(m => (m.Street != null) &&
                (m.PostCode != null) &&
                !m.Street.Contains(m.PostCode),
        "Both street and postcode are required and need to put separate");
```

``` csharp
Validator<UserModel> userValidator = specs => specs
    // Validate members with the predefined rules:
    .For(m => m.Email, be => be.Email())

    // Apply many rules along with custom predicates:
    .For(m => m.Name, be => be
            // By default, everything specified is required, so marking the selected member as optional:
            .Optional()
            // If present, proceed with validation:
            .LengthBetween(6, 15)
            // The value is always guaranteed to be non-null inside the predicate:
            .Valid(v => char.IsLetter(v.FirstOrDefault()), "Must start with a letter")
            .Valid(v => v.All(char.IsLetterOrDigit), "Must contains only letters and digits"))

    // Replace all errors with a single one:
    .For(m => m.Password, be => be
        .MinLength(6)
        .NotWhiteSpace()
        .Valid(v => v.Any(char.IsUpper) && v.Any(char.IsDigit), string.Empty)
        // If any rule in the chain fails, only the SummaryError is recorded:
        .WithSummaryError("Minimum 6 characters, at least one upper case and one digit"))

    // Validate relations with other members:
    .For(m => m.PasswordConfirmation, be => be
        // Override the name of the selected member:
        .WithName("Confirmation")
        // Argument in predicate is the parent model, but error will be attached in the selected member scope:
        .ValidRelative(m => m.Password == m.PasswordConfirmation,
            "Confirmation doesn't match password"))

    // Validate nested model:
    .For(m => m.Address, be => be.ValidModel(addressValidator))

    // Validate collection:
    .For(m => m.Tags, be => be
        // Override default message for predefine rule:
        .NotEmpty(message: "At least one tag is required")
        // All rule arguments can be inserted in the message using {argumentName} pattern:
        .MaxSize(max: 5, message: "Max {max} tags allowed")
        // Validate every item inside of the collection:
        .ValidCollection(i => i
            .NotWhiteSpace()
            .MaxLength(10)
            .Valid(v => v.All(char.IsLetter), "Tag can contains only letters")))

    // Validate nullables:
    .For(m => m.DateOfBirth, be => be
        // Override default RequiredError for selected member:
        .WithRequiredError("Date of birth is required")
        // Arguments could be parametrized:
        .After(value: new DateTime(1900, 1, 1), message: "Earliest allowed date is {value|format=yyyy-MM-dd}" ));
```

### Create validation context

Use a factory to create and configure `IValidationContext` instance (which itself is immutable), so you can keep it e.g. within your bounded context or CQRS feature

``` csharp
var validationContext = ValidationContext.Factory.Create(options => options
    // Add validators for all models (including nested ones)
    .AddValidator(userValidator)
    .AddValidator(addressValidator)

    // Add translations (to have possibility to serve results in different language), e.g. Polish
    .AddPolishTranslation(asDefault: false)

    // Add phrases to Polish translation - e.g. for the custom messages used in userValidator
    // Translation is a standard dictionary, so you can easily deserialize it from JSON, or create inline like:
    .AddTranslation("Polish", new Dictionary<string, string>
    {
        // Translation for a phrase is a simple KeyValuePair
        {"Date of birth is required", "Data urodzenia jest wymagana"},

        // ...

        // You can use arguments in translations...
        {"Max {max} tags allowed", "Maksymalnie dozwolonych jest {max} tagów"},

        // ... and parametrize them differently (e.g. change the format, culture, etc.)
        {"Earliest allowed date is {value|format=yyyy-MM-dd}", "Najwcześniejsza dozwolona data to {value|format=dd.MM.yyyy}"}
    })

    // Set additional options (lot of them available...)
    .SetValidationStategy(ValidationStrategy.Complete)
    .SetNullRootStrategy(NullRootStrategy.ArgumentNullException)
    .SetMaxDepth(5)
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
    Name = null,
    Password = "12345",
    PasswordConfirmation = "1234567",
    Address = new AddressModel
    {
        Street = null,
        PostCode = "1234-5678-90",
        CountryId = -1
    },
    Tags = new[]
    {
        "validtag",
        "inv@lid",
        " ",
        null,
        "oktag",
        null
    },
    DateOfBirth = null
};
```

The outcome of `JsonConvert.SerializeObject(modelReport)` would be

``` json
{
    "Email": ["Text value should be a valid email"],
    "Password": ["Minimum 6 characters, at least one upper case and one digit"],
    "Confirmation": ["Confirmation doesn't match password"],
    "Address": {
        "": ["Both street and postcode are required and need to put separate"],
        "Street": ["Required"],
        "PostCode": ["Text value should have maximum 10 characters"],
        "CountryId": ["Number should be greater than 0"]
    },
    "Tags": {
        "": ["Max 5 tags allowed"],
        "1": ["Tag can contains only letters"],
        "2": ["Text value cannot be whitespace", "Tag can contains only letters"],
        "3": ["Required"],
        "5": ["Required"]
    },
    "DateOfBirth": ["Date of birth is required"]
}
```

Variety of other actions are available, like `ListReport`, which is a collection of strings

``` csharp
var listReport = result.ToListReport();
```

You can enumerate over `listReport` to read the error messages one by one or combine them all using  `listReport.ToString()`

```
Email: Text value should be a valid email
Password: Minimum 6 characters, at least one upper case and one digit
Confirmation: Confirmation doesn't match password
Address: Both street and postcode are required and need to put separate
Address.Street: Required
Address.PostCode: Text value should have maximum 10 characters
Address.CountryId: Number should be greater than 0
Tags: Max 5 tags allowed
Tags.1: Tag can contains only letters
Tags.2: Text value cannot be whitespace
Tags.2: Tag can contains only letters
Tags.3: Required
Tags.5: Required
DateOfBirth: Date of birth is required
```

You can perform a validation with a different set of options

``` csharp
// Adjust ValidationOptions for a single validation
var failFastResult = validationContext.Validate(userModel, options => options
    // You can override all options of the IValidationContext (except validators and translations):
    // Like setting `FailFast` strategy - so the model will be validated until the first error
    .SetValidationStategy(ValidationStrategy.FailFast)
);

var failFastModelReport = failFastResult.ToModelReport();
```

This time, `JsonConvert.SerializeObject(failFastModelReport)` gives us

``` json
{
  "Email": ["Text value should be a valid email"]
}
```

Having the result, you can e.g. generate report in different language (don't forget to include the translation when constructing `IValidationContext`)

``` csharp
// All report creators provide `translationName` optional argument
var listReportInPolish = failFastResult.ToListReport(translationName: "Polish")
```

Unsurprisingly, `listReportInPolish.ToString()` results with

```
Email: Wartość tekstowa powinna zawierać prawidłowy adres email
```

Generating a report is, of course, not required at all

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

Want to see this example in action and debug it for yourself? Go ahead, it exists as a [functional test](tests/CoreValidation.FunctionalTests/Quickstart/QuickstartTests.cs).

## What's more

Briefly, with CoreValidation you can also:

* Clone `IValidationContext` and modify its options on the fly (e.g. add validators, translations, set different default values)
* Merge two (or more) results, so it's perfectly possible to distribute the work to multiple `IValidationContext` (e.g. each in different thread validating its part simultaneously)
* Select `ValidationStrategy.Force` to gather all possible errors for the model (e.g. to have a full specification of a valid model, ready to be copy-pasted to some documentation or swagger)
* Easily extend it as you want; custom rules, additional translations, new report types (any contribution to this repository is more than welcomed!)

All this (and more) is ~~documented on the wiki page~~ (wiki is under construction)
