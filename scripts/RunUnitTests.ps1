Param (
    [bool]$build = $true,
    [bool]$checkCoverage = $true,
    [string]$coverageReportTag = "",
    [bool]$zipCoverageReport = $false
)

$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

if ($build) {
    Exec "Building before unit tests" {
        & $scriptsPath\Build.ps1 -configuration Debug
    }
}

$rootDir = Convert-Path "$($scriptsPath)\..\"
$testsPath = Convert-Path "$($rootDir)\tests"
$coveragePath = "$($rootDir)\artifacts\coverage"
$toolsPath = "$($rootDir)\tools"

if ($checkCoverage) {
    New-Item -ItemType Directory -Force -Path $coveragePath
    $coveragePath = Convert-Path $coveragePath
}

Exec "Unit tests" {
    & dotnet test $testsPath\CoreValidation.UnitTests\CoreValidation.UnitTests.csproj -c Debug --no-build /p:CollectCoverage=$checkCoverage /p:CoverletOutput=$coveragePath\ /p:CoverletOutputFormat=opencover
}

if ($checkCoverage) {
    New-Item -ItemType Directory -Force -Path $toolsPath
    $toolsPath = Convert-Path $toolsPath

    $coverageReportPath = "$($coveragePath)\report"

    New-Item -ItemType Directory -Force -Path $coverageReportPath
    $coverageReportPath = Convert-Path $coverageReportPath
3
    $reportGeneratorToolItem = TryGetToolExecutableItem $toolsPath "dotnet-reportgenerator-globaltool" "4.0.0-rc4" "ReportGenerator.dll"

    if (($null -eq $reportGeneratorToolItem)) {
        & dotnet tool install "dotnet-reportgenerator-globaltool" --tool-path $toolsPath --version "4.0.0-rc4"
        $reportGeneratorToolItem = TryGetToolExecutableItem $toolsPath "dotnet-reportgenerator-globaltool" "4.0.0-rc4" "ReportGenerator.dll"
    }

    Exec "Generating report" {
        $opencoverReport = Convert-Path $coveragePath\coverage.opencover.xml
        & dotnet ($reportGeneratorToolItem.FullName) -- "-reports:$($opencoverReport)" "-targetdir:$($coverageReportPath)\" "-verbosity:Info" "-tag:$($coverageReportTag)"
    }

    if ($zipCoverageReport) {
        $zipReportPath = Join-Path $coveragePath report.zip

        Exec "Packing report" {
            & Compress-Archive -Path $coverageReportPath\* -DestinationPath $zipReportPath
        }

        Exec "Cleaning report files" {
            & Remove-Item -Path $coverageReportPath -Recurse -Force
        }
    }
}
