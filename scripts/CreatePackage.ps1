Param (
    [Parameter(Position = 0, Mandatory = $true)][string]$version,
    [Parameter(Position = 1, Mandatory = $false)][bool]$build = $true
)

$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

if ($build) {
    Exec "Building before creating package" {
        & $scriptsPath\Build.ps1 -configuration Release
    }
}

$projectFilePath = Convert-Path "$($scriptsPath)\..\src\CoreValidation\CoreValidation.csproj"
$nugetPath = "$($scriptsPath)\..\artifacts\nuget"

New-Item -ItemType Directory -Force -Path $nugetPath

$nugetPackagePath = "$($nugetPath)\CoreValidation.$($version).nupkg"

if (Test-Path $nugetPackagePath) {
    Exec "Cleaning existing artifacts" {
        Remove-Item $nugetPackagePath
    }
}

Exec "Packing version $($version)" {
    & dotnet pack $projectFilePath -c Release --no-build -o $nugetPath /p:Version=$version
}