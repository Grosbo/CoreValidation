Param (
    [bool]$build = $true
)

$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

if ($build) {
    Exec "Building before testing" {
        & ($scriptsPath + "\Build.ps1")
    }
}

$rootDir = Convert-Path "$($scriptsPath)\..\"
$testsDir = Convert-Path "$($rootDir)\tests"

Exec "Functional tests" {
    & dotnet test $testsDir\CoreValidation.FunctionalTests\CoreValidation.FunctionalTests.csproj -c Release --no-build
}

