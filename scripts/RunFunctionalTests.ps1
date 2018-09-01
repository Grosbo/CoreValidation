Param (
    [bool]$build = $true
)

$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

if ($build) {
  Exec "Building before functional tests" {
    & $scriptsPath\Build.ps1 -configuration Debug
  }
}

$rootPath = Convert-Path "$($scriptsPath)\..\"
$testsPath = Convert-Path "$($rootPath)\test"

Exec "Functional tests" {
  & dotnet test $testsPath\CoreValidation.FunctionalTests\CoreValidation.FunctionalTests.csproj -c Debug --no-build
}
