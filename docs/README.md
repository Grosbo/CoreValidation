# CoreValidation

![CoreValidation logo](https://github.com/bartoszlenar/CoreValidation/raw/master/logo/logo_128.png)

[![Build status](https://ci.appveyor.com/api/projects/status/y9itfpoy8su7yuxk/branch/master?svg=true)](https://ci.appveyor.com/project/bartoszlenar/corevalidation/branch/master) [![Coverage Status](https://coveralls.io/repos/github/bartoszlenar/CoreValidation/badge.svg)](https://coveralls.io/github/bartoszlenar/CoreValidation) [![NuGet package](https://img.shields.io/nuget/v/CoreValidation.svg)](https://www.nuget.org/packages/CoreValidation)

## About

**Advanced model validation for the modern dotnet world** - indeed, this is the tagline, but what does the library really do?

* Easily handles advanced validation scenarios for complex models within your app
* Delivers friendly fluent api to specify a valid state of model with its reference and value types, nested models, nullables, collections and relations between all of them
* Only 70KB, based on .NET Standard 2.0, no extra dependencies
* Designed to work well with DDD\CQRS approaches
* Supports variety of report types and translations out of the box
* Extensibility points at every level for advanced behavior customization
* Published under MIT licence - the most permissive one that exist

A code is worth a thousand words - so keep scrolling this document down and you will find two examples under the rather self-descriptive names:

* [Quickstart](#quickstart) - for those who want to see only the basic usage
* [End-to-end](#end-to-end) - for those looking for the advanced scenario presenting variety of available features and the elegant way of integrating them within the project

For more, read the [documentation](DOCUMENTATION.md).

Moreover, all the code snippets are being maintained as the [functional tests](../test/CoreValidation.FunctionalTests), so you can debug them and examine the behavior in no time.

## Release notes

### 1.0.0-beta - July 2nd, 2018

* First public version, the starting point

Read the full [release notes document](RELEASE_NOTES.md).

## Quickstart

For a start, let's quickly cover the entry point of many web apps; logging the user.

``` csharp
// Setting the rules for the model:
Specification<LoginModel> loginSpecification = login => login
    .For(m => m.Email, be => be
        .Email()
        .MaxLength(50)
        .Valid(email => email.EndsWith("gmail.com"), "Only gmails are accepted"))
    .For(m => m.Password, be => be.NotEmpty());

var loginModel = JsonConvert.DeserializeObject<LoginModel>(incomingJson);

// Creating the validation context and instantly triggering the validation to get the result:
var result = ValidationContext.Factory
    .Create(options => options.AddSpecification(loginSpecification))
    .Validate(loginModel);

// Creating json-ready ModelReport from the result:
var modelReport = result.ToModelReport();
```

So, when the incoming JSON is:

``` json
{
    "Email": "sample_very_long_email@deep.domain.level.tempuri.org",
    "Password": ""
}
```

The output of `JsonConvert.SerializeObject(modelReport)` would be:

``` json
{
    "Email": [
        "Text value should have maximum 50 characters",
        "Only gmails are accepted"
    ],
    "Password": [
        "Text value cannot be empty"
    ]
}
```

Functional test: [Quickstart.cs](../test/CoreValidation.FunctionalTests/Readme/Quickstart.cs).


## End-to-end

For a detailed example, let's handle something more juicy. If the logging the user scenario is covered, how about signing up?

Let's have a `UserModel` class for creating a new user in the app. Within it we're going to validate:

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

### Install

The package is hosted on [nuget.org](https://www.nuget.org/packages/CoreValidation), so to add it to your project, by default this is good enough:

``` bash
dotnet add package CoreValidation --version 1.0.0-beta
```

### Define specifications

You can create them inline or in separate classes - it's up to you how you would like to structure your code

``` csharp
Specification<AddressModel> addressSpecification = specs => specs
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
Specification<UserModel> userSpecification = specs => specs
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
        .Valid(v => v.Any(char.IsUpper) && v.Any(char.IsDigit))
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
    .For(m => m.Address, be => be.ValidModel(addressSpecification))

    // Validate collection:
    .For(m => m.Tags, be => be
        // Override default message for predefine rule:
        .NotEmptyCollection(message: "At least one tag is required")
        // All rule arguments can be inserted in the message using {argumentName} pattern:
        .MaxCollectionSize(max: 5, message: "Max {max} tags allowed")
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
        .After(min: new DateTime(1900, 1, 1), message: "Earliest allowed date is {min|format=yyyy-MM-dd}"));
```

### Create validation context

Use a factory to create and configure `IValidationContext` instance (which itself is immutable), so you can keep it e.g. within your bounded context or CQRS feature

``` csharp
var validationContext = ValidationContext.Factory.Create(options => options
    // Add specifications for all models to validate (including nested ones)
    .AddSpecification(userSpecification)
    .AddSpecification(addressSpecification)

    // Add translations (to have possibility to serve results in different language), e.g. Polish
    .AddPolishTranslation(asDefault: false, include: new Dictionary<string, string>
    {
        // Add more phrases to Polish translation - e.g. for the custom messages used in userSpecification
        // Translation is a standard dictionary, so you can easily deserialize it from JSON, or create inline like:
        {"Date of birth is required", "Data urodzenia jest wymagana"},

        // Override the default entry (static Phrases.Keys holds all default keys for the phrases)
        {Phrases.Keys.Texts.Email, "Email jest wymagany"},

        // You can use arguments in translations...
        {"Max {max} tags allowed", "Maksymalnie dozwolonych jest {max} tagów"},

        // ... and parametrize them differently (e.g. change the format, culture, etc.)
        {"Earliest allowed date is {min|format=yyyy-MM-dd}", "Najwcześniejsza dozwolona data to {min|format=dd.MM.yyy}"}
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

### Create the JSON-friendly report (ModelReport)

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

### Create the logs-friendly report (ListReport)

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

### Override default validation options

You can perform a validation with a different set of options

``` csharp
// Adjust ValidationOptions for a single validation
var failFastResult = validationContext.Validate(userModel, options => options
    // You can override all options of the IValidationContext (except specifications and translations):
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

### Translate the report

Having the result, you can e.g. generate report in different language (don't forget to include the translation when constructing the instance of `IValidationContext`)

``` csharp
// All report creators provide `translationName` optional argument
var listReportInPolish = failFastResult.ToListReport(translationName: "Polish")
```

Unsurprisingly, `listReportInPolish.ToString()` results with

```
Email: Email jest wymagany
```

### Act basing on the result

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

Functional test with this scenario: [EndToEnd.cs](../test/CoreValidation.FunctionalTests/Readme/EndToEnd.cs).


## What's more

Although the two scenarios above are presenting most of the features, CoreValidation allows a lot more, including:

* Cloning the instance of `IValidationContext` and modifying its options on the fly - to add new validators, translations or changing the defaults
* Merging multiple results into one, so it's perfectly possible to distribute the work across multiple validation contexts (e.g. to simultaneously handle different parts of the model) and combine one final report from all of the results
* Using `ValidationStrategy.Force` to gather all possible errors for the model - handy if you want to have a full specification of a valid model, ready to be copy-pasted into some documentation page or swagger
* Extending it at every point; custom sophisticated rules, additional translations or error codes, new report formats - everything is as easy as writing an extension method

Take a look at the [documentation](DOCUMENTATION.md). It's all covered there.
