Param (
    [bool]$build = $true
)

$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

if ($build) {
    Exec "Building before testing" {
        & $scriptsPath\Build.ps1
    }
}

$rootDir = Convert-Path "$($scriptsPath)\..\"
$testsDir = Convert-Path "$($rootDir)\tests"

Exec "Unit tests" {
    & dotnet test $testsDir\CoreValidation.UnitTests\CoreValidation.UnitTests.csproj -c Release --no-build
}

Exec "Unit tests (predefined rules)" {
    & dotnet test $testsDir\CoreValidation.PredefinedRules.UnitTests\CoreValidation.PredefinedRules.UnitTests.csproj -c Release --no-build
}
