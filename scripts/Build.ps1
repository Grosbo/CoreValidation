$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

Exec "dotnet SDK" {
    & dotnet --version
}

$rootDir = Convert-Path "$($scriptsPath)\..\"
$solutionFile = Convert-Path "$($rootDir)\CoreValidation.sln"

Exec "Cleaning builds" {
    & dotnet clean $solutionFile -c Release --verbosity m
}

Exec "Building" {
    & dotnet build $solutionFile -c Release --no-incremental
}
