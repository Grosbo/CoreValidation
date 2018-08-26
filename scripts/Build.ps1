Param (
    [Parameter(Position = 0, Mandatory = $false)][string]$configuration = "Debug"
)

$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

Print "Bulding: $($configuration)"

Exec "dotnet SDK" {
    & dotnet --version
}

$rootPath = Convert-Path "$($scriptsPath)\..\"
$solutionFile = Convert-Path "$($rootPath)\CoreValidation.sln"

Exec "Cleaning builds" {
    & dotnet clean $solutionFile -c $configuration --verbosity m
}

Exec "Building" {
    & dotnet build $solutionFile -c $configuration --no-incremental
}
