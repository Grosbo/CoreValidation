Param (
    [Parameter(Position = 0, Mandatory = $true)][string]$path,
    [Parameter(Position = 1, Mandatory = $true)][string]$nugetApiKey
)

$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

Exec "Publishing: $($path)" {
    & dotnet nuget push $path -k $nugetApiKey -s https://api.nuget.org/v3/index.json
}