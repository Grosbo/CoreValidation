Param (
    [Parameter(Position = 0, Mandatory = $true)][string]$version,
    [bool]$build = $true
)

$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

if ($build) {
    Exec "Building before packing" {
        & $scriptsPath\Build.ps1
    }
}

$projectFilePath = Convert-Path "$($scriptsPath)\..\src\CoreValidation\CoreValidation.csproj"
$artifactsDirPath = "$($scriptsPath)\..\artifacts"

New-Item -ItemType Directory -Force -Path $artifactsDirPath

$nugetPackagePath = "$($artifactsDirPath)\CoreValidation.$($version).nupkg"

Exec "Cleaning existing artifacts" {
  if (Test-Path $nugetPackagePath) {
      Remove-Item $nugetPackagePath
  }
}

Exec "Packing version $($version)" {
    & dotnet pack $projectFilePath -c Release --no-build -o $artifactsDirPath /p:Version=$version
}