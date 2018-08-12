$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

Exec "Building before testing" {
    & $scriptsPath\Build.ps1
}

$rootDir = Convert-Path "$($scriptsPath)\..\"
$testsDir = Convert-Path "$($rootDir)\tests"

$artifactsDir = Convert-Path "$($rootDir)\artifacts"

New-Item -ItemType Directory -Force -Path $artifactsDir

$benchmarkArtifactsDir = "$($artifactsDir)\benchmark_$([DateTime]::UtcNow.ToString('yyyyMMddHHmmss'))"

New-Item -ItemType Directory -Force -Path $benchmarkArtifactsDir

$testNames = @(
    "Messages.*"
    "Readme.*"
)

for ($i = 0; $i -lt $testNames.length; $i++) {
    Exec "Running: $($testNames[$i])" {
        & dotnet run --project $testsDir\CoreValidation.PerformanceTests\CoreValidation.PerformanceTests.csproj -c Release -- --filter "*$($testNames[$i])" --artifacts=$benchmarkArtifactsDir
    }
}