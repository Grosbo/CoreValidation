<h1 align="center">
  <br />
    <img src="logo/CoreValidation-logo.svg" alt="CoreValidation logo" height="256px" width="256px" />
  <br />
  CoreValidation
  <br />
</h1>

<h4 align="center">Advanced model validation library for the modern <a href="https://www.microsoft.com/net" target="_blank">dotnet</a> world.</h4>

<p align="center">
  <a href="https://ci.appveyor.com/project/bartoszlenar/corevalidation/branch/master">
    <img src="https://img.shields.io/appveyor/ci/bartoszlenar/CoreValidation/master.svg?&style=for-the-badge" alt="Build status" />
  </a>
  <a href="https://ci.appveyor.com/project/bartoszlenar/corevalidation/branch/master">
    <img src="https://img.shields.io/appveyor/tests/bartoszlenar/CoreValidation/master.svg?&style=for-the-badge" alt="Tests status" />
  </a>
  <a href="https://coveralls.io/github/bartoszlenar/CoreValidation">
    <img src="https://img.shields.io/coveralls/github/bartoszlenar/CoreValidation/master.svg?&style=for-the-badge" alt="Coverage Status" />
  </a>
  <a href="https://www.nuget.org/packages/CoreValidation">
      <img src="https://img.shields.io/nuget/v/CoreValidation.svg?&style=for-the-badge" alt="NuGet package" />
  </a>
</p>


## Features

Briefly:

* Fluent API everywhere
  * no need of creating separate classes with just validation logic (although it's supported as well)
* Validates complex models
  * classes, value types, nullables, nested models, collections and relations between them all
  * all combinations of the above
* Strategies
  * including Force which can help with documentation by collecting all theoretically possible errors
* Global model errors
  * ability to assign an error to the model itself instead of its members
* Predefined rules
  * validate numbers, strings, dates and times out-of-the box
* Localizations
  * with custom translations and formattable parameters
* Reporting
  * different types (e.g. model-shaped for JSON API response or list of messages for the logs)
  * independent from validation process (validate once - produce many different reports in different formats and languages)
* Partial validation
  * handy when validation process involves remote calls or multiple threads
* Extendable
  * create your own reusable rules, reports, translations, etc.

Technically:

* Designed to fit in DDD\CQRS code architectures
* Contained in 70KB [nuget package](https://www.nuget.org/packages/CoreValidation)
* Pure [.NET Standard 2.0](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md), no extra dependencies
* [MIT licence](https://github.com/bartoszlenar/CoreValidation/blob/master/LICENSE)


## Installation

The package is hosted on [nuget.org](https://www.nuget.org/packages/CoreValidation), so to add it to the project, by default this is good enough:

``` bash
dotnet add package CoreValidation
```

### Versioning

* Version numbers strictly follow the [semantic versioning](https://semver.org/spec/v2.0.0.html) rules.
* All changes are detailly logged in the separate release notes document.

## Quickstart

### 1. Know your model

``` csharp
class SignUpModel
{
  public string Name { get; set; }
  public string Email { get; set; }
  public string Password { get; set; }
  public string PasswordConfirmation { get; set; }
  public bool? TermsAndConditionsConsent { get; set; }
}
```

Classic model for signing up the user. Just one nullable `bool?` and four `string` fields. All with rather self-descriptive names.

### 2. Specify its valid state

``` csharp
using CoreValidation;
```

``` csharp
Specification<SignUpModel> signUpSpecification = s => s
  .Member(m => m.Name, m => m
      .SetOptional()
      .MaxLength(40)
  )
  .Member(m => m.Email, m => m
      .SetOptional()
      .Email()
      .MaxLength(40)
  )
  .Member(m => m.Password, m => m
      .NotWhiteSpace()
      .MinLength(min: 10).WithMessage("Password should contain at least {min} characters")
      .Valid(p => p.Any(char.IsDigit)).WithMessage("Password should contain at least one digit")
  )
  .Member(m => m.PasswordConfirmation, m => m
      .AsRelative(n => n.Password == n.PasswordConfirmation).WithMessage("Confirmation doesn't match the password")
  )
  .Member(m => m.TermsAndConditionsConsent)
  .Valid(m => (m.Name != null) || (m.Email != null)).WithMessage("At least one value is required - Name or Email");
```

#### Member scope

Errors in member scopes (the one defined in `Member` command) are assigned to the selected members.

* Until explicitly set as optional with `SetOptional`, by default every member listed in specification is marked as required.
* `Name` is optional, but if present - it needs to contain no more than 40 characters.
* `Email` is also optional and needs to be a valid email address, maximum 40 characters long.
* `Password` has minimum length of 10 characters and must include at least one digit.
  * `MinLength` rule accepts the argument (`min`), that could be used in `WithMessage` to override the default error message.
  * `Valid` is the way to set custom validation logic.
* `PasswordConfirmation` needs to be the same as `Password`.
  * `AsRelative` rule exposes the parent model so you can validate the relations between its fields.
* `TermsAndConditionsConsent` has no rules, but it's required (cannot be null).

#### Model scope

Errors will be assigned to the global model scope. Useful for errors that are related to the model as a whole and not directly to any of its members.

* Same as in member scope, `Valid` accepts the predicate that can hold a custom validation logic.
* Both `Name` and `Email` are optional, but when both are null - the entire model doesn't have any sense and it's invalid.

### 3. Create validation context

``` csharp
var validationContext = ValidationContext.Factory.Create(options => options
  .AddSpecification(signUpSpecification)
);
```

`ValidationContext` object is the entry point for the validation process - it can validate the objects against the registered specifications. It is immutable and thread-safe and can be safely registered as a singleton in the DI container or stored as a static class member.

It is recommended to have many instances of `ValidationContext`, e.g. for each:

* bounded context (or CQRS feature)
* architectural layer that requires a valid model on input (like controllers, business logic services, etc.)
* part of the model that meant to be validated simultaneously (results can be merged later)
* specific validation scenario (plenty of options available)

### 4. Validate the object

Lets assume that `SignUpModel` object is created in the microservice from incoming HTTP request:

``` json
{
  "Password": "homerBEST",
  "PasswordConfirmation": "homerbest"
}
```

After deserializing the JSON, validation of `signUpModel` is straightforward:

``` csharp
var validationResult = validationContext.Validate(signUpModel);
```

### 5. Process the result

#### Flag

Having the result (`IValidationResult` instance), you can check if the model is valid using `IsValid` property:

``` csharp
var isSignUpModelValid = validationResult.IsValid; // false
```

#### Exception

Or block the code execution by throwing result in the exception:

``` csharp
validationResult.ThrowResultIfInvalid(); // throws ValidationResultException<SignUpModel>
```

#### List Report

Call `ToListReport` to get a list of error messages, e.g. to log them:

``` csharp
var listReport = validationResult.ToListReport();
```

`listReport.ToString()` output:

```
At least one value is required - Name or Email
Password: Password should contain at least 10 characters
Password: Password should contain at least one digit
PasswordConfirmation: Confirmation doesn't match the password
TermsAndConditionsConsent: Required
```

#### Model Report

Call `ToModelReport` to get the report that could be serialized and returned in the HTTP response:

``` csharp
var modelReport = validationResult.ToModelReport();
```

`JsonConvert.SerializeObject(modelReport)` output:

``` json
{
  "Email": [
    "Text value should have maximum 40 characters"
  ],
  "Password": [
    "Password should contain at least 10 characters",
    "Password should contain at least one digit"
  ],
  "PasswordConfirmation": [
    "Confirmation doesn't match the password"
  ],
  "TermsAndConditionsConsent": [
    "Required"
  ],
  "": [
    "At least one value is required - Name or Email"
  ]
}
```

### 6. Debug it yourself!

Each example (here and in [documentation](https://github.com/bartoszlenar/CoreValidation/wiki)) lives as a functional test. To navigate to it, look for the check mark next to the filename:

[:ballot_box_with_check: Readme_Showcase_Test.cs](test/CoreValidation.FunctionalTests/Readme/Readme_Showcase_Test.cs)

## Documentation

The documentation is hosted as a [GitHub wiki](https://github.com/bartoszlenar/CoreValidation/wiki).

## Author

Initially created by [Bartosz Lenar](http://bartoszlenar.net/)