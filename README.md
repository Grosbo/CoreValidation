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
  * no need of creating separate classes with just validation logic (although [it's supported](https://github.com/bartoszlenar/CoreValidation/wiki) as well)
* Validates complex models
  * [classes](https://github.com/bartoszlenar/CoreValidation/wiki), [value types](https://github.com/bartoszlenar/CoreValidation/wiki), [nullables](https://github.com/bartoszlenar/CoreValidation/wiki), [nested models](https://github.com/bartoszlenar/CoreValidation/wiki), [collections](https://github.com/bartoszlenar/CoreValidation/wiki) and [relations](https://github.com/bartoszlenar/CoreValidation/wiki) between them all
  * all [combinations](https://github.com/bartoszlenar/CoreValidation/wiki) of the above
* [Strategies](https://github.com/bartoszlenar/CoreValidation/wiki)
  * including [Force](https://github.com/bartoszlenar/CoreValidation/wiki) which can help with documentation by collecting all theoretically possible errors
* Global [model errors](https://github.com/bartoszlenar/CoreValidation/wiki)
  * ability to assign an error to the model itself instead of its members
* [Predefined rules](https://github.com/bartoszlenar/CoreValidation/wiki)
  * validate [numbers](https://github.com/bartoszlenar/CoreValidation/wiki), [strings](https://github.com/bartoszlenar/CoreValidation/wiki), [dates](https://github.com/bartoszlenar/CoreValidation/wiki) and [times](https://github.com/bartoszlenar/CoreValidation/wiki) out-of-the box
* [Localizations](https://github.com/bartoszlenar/CoreValidation/wiki)
  * with [custom translations](https://github.com/bartoszlenar/CoreValidation/wiki) and [formattable parameters](https://github.com/bartoszlenar/CoreValidation/wiki)
* [Reporting](https://github.com/bartoszlenar/CoreValidation/wiki)
  * different types (e.g. [model-shaped](https://github.com/bartoszlenar/CoreValidation/wiki) for JSON API response or [list of messages](https://github.com/bartoszlenar/CoreValidation/wiki) for the logs)
  * independent from validation process (validate once - produce many different reports in different formats and languages)
* [Partial validation](https://github.com/bartoszlenar/CoreValidation/wiki)
  * handy when validation process involves remote calls or multiple threads
* [Extendable](https://github.com/bartoszlenar/CoreValidation/wiki)
  * create your own [reusable rules](https://github.com/bartoszlenar/CoreValidation/wiki), [reports](https://github.com/bartoszlenar/CoreValidation/wiki), [translations](https://github.com/bartoszlenar/CoreValidation/wiki), etc.

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
* All changes are detailly logged in the separate [release notes document](https://github.com/bartoszlenar/CoreValidation/wiki).

## Quickstart

### 1. Know your model

``` csharp
class SignUpModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
    public bool? TermsAndConditionsConsent { get; set; }
}
```

Classic model for signing up the user. Just one nullable `bool?` and three `string` fields.

### 2. Specify its valid state

``` csharp
Specification<SignUpModel> signUpModelSpecification = s => s

    .Member(m => m.Email, m => m
        .Email()
        .MaxLength(40))

    .Member(m => m.Password, m => m
        .MinLength(min: 10).WithMessage("Password should contain at least {min} characters")
        .Valid(p => p.Any(char.IsDigit)).WithMessage("Password should contain at least one digit"))

    .Member(m => m.PasswordConfirmation, m => m
        .AsRelative(n => n.Password == n.PasswordConfirmation).WithMessage("Invalid confirmation"))

    .Member(m => m.TermsAndConditionsConsent)

    .Valid(m => m.TermsAndConditionsConsent == true).WithMessage("Without the consent, sign up is invalid");

```

#### `Member` - specification for member fields:

* `Email` has two [predefined rules](https://github.com/bartoszlenar/CoreValidation/wiki) with rather self-descriptive names (you can also [create your own](https://github.com/bartoszlenar/CoreValidation/wiki)).
* `Password` has two rules:
  * Predefined `MinLength` with `min` [parameter](https://github.com/bartoszlenar/CoreValidation/wiki) which is also used as a [placeholder](https://github.com/bartoszlenar/CoreValidation/wiki) in the [message](https://github.com/bartoszlenar/CoreValidation/wiki) that overrides the default one.
  * `Valid` - the way to include an in-line predicate as a [custom rule](https://github.com/bartoszlenar/CoreValidation/wiki), also [with a message](https://github.com/bartoszlenar/CoreValidation/wiki).
* `PasswordConfirmation` - `AsRelative` delivers the parent model so you can [validate the relations](https://github.com/bartoszlenar/CoreValidation/wiki) between its fields.
* `TermsAndConditionsConsent` has no rules, but everything listed in `Specification` will be marked as required by default.

#### `Valid` - Specification for the model itself

* A single custom rule for the model as a whole.

### 3. Create validation context

``` csharp
var validationContext = ValidationContext.Factory.Create(options => options
    .AddSpecification(signUpModelSpecification)
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
  "Email": "homer.jay.simpson@emailaccount.tempuri.org",
  "Password": "homerBEST",
  "PasswordConfirmation": "homerbest"
}
```

After deserializing the JSON, validation of `signUpModel` is straightforward:

``` csharp
var validationResult = validationContext.Validate(signUpModel);
```

### 5. Process the result

Having the `IValidationResult` instance, you can check if the model is valid using `IsValid`:

``` csharp
var isSignUpModelValid = validationResult.IsValid; // false
```

Call `ToListReport` to get a list of error messages, e.g. to log them:

``` csharp
var listReport = validationResult.ToListReport();
```

`listReport.ToString()` output:

```
Without the consent, sign up is invalid
Email: Text value should have maximum 40 characters
Password: Password should contain at least 10 characters
Password: Password should contain at least one digit
PasswordConfirmation: Confirmation doesn't match the password
TermsAndConditionsConsent: Required
```

Call `ToModelReport` to get the report that could be serialized and returned in the HTTP response (and lets twist it a little bit by using Polish translation):

``` csharp
var modelReport = validationResult.ToModelReport(translationName: nameof(Phrases.Polish));
```

`JsonConvert.SerializeObject(modelReport)` output:

``` json
{
  "Email": [
    "Text value should have maximum 40 characters"
  ],
  "Password": [
    "Password should contain at least 10 characters",
    "Password should contain at least one digit",
  ],
  "PasswordConfirmation": [
    "Confirmation doesn't match the password"
  ],
  "TermsAndConditionsConsent": [
    "Required"
  ],
  "": [
    "Without the consent, sign up is invalid"
  ]
}
```


### 6. Debug it yourself!

Each example lives as a functional test. To navigate to it, look for the green check mark next to the filename:

[:heavy_check_mark: SignUp.cs](test/CoreValidation.FunctionalTests/QuickStart/SignUp.cs)

### 7. Discover more

For more advanced scenarios of usage, follow to [this page](https://github.com/bartoszlenar/CoreValidation/wiki).

The documentation is hosted as a [GitHub wiki](https://github.com/bartoszlenar/CoreValidation/wiki).

## Author

Initially created by [Bartosz Lenar](http://bartoszlenar.net/)