$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

$rootPath = Convert-Path "$($scriptsPath)\..\"
$testsPath = Convert-Path "$($rootPath)\test"

Exec "Building before performance tests" {
  & $scriptsPath\Build.ps1 -configuration Release
}

$benchmarkPath = "$($rootPath)\benchmarks\$([DateTime]::Now.ToString('yyyyMMdd-HHmmss'))"

New-Item -ItemType Directory -Force -Path $benchmarkPath
$benchmarkPath = Convert-Path $benchmarkPath

$testNames = @(
    "MessagesBenchmark*"
    "RulesBenchmark*"
    "ReportsBenchmark*"
)

for ($i = 0; $i -lt $testNames.length; $i++) {
    Exec "Running: $($testNames[$i])" {
        & dotnet run --project $testsPath\CoreValidation.PerformanceTests\CoreValidation.PerformanceTests.csproj -c Release --no-build -- --filter "*$($testNames[$i])" --artifacts=$benchmarkPath
    }
}