$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

$environmentInfo = . $scriptsPath\GetEnvironmentInfo.ps1

if ($environmentInfo.IsServer -eq $false) {
    Print "!!! Server environment not detected !!!"
}

if ($environmentInfo.IsCommit -eq $false) {
    Print "!!! Commit not detected !!!"
}
else {
    Print "COMMIT:"
    Print "Hash: $($environmentInfo.CommitHash)"
    Print "Date: $($environmentInfo.CommitDate)"
    Print "Author: $($environmentInfo.CommitAuthor)"
    Print "Message: $($environmentInfo.CommitMessage)"

    if ($environmentInfo.IsTagged) {
        Print "TAG:"
        Print "Name: $($environmentInfo.TagName)"
        Print "Version: $($environmentInfo.TagVersion)"
    }

    if ($environmentInfo.IsPullRequest) {
        Print "PULL REQUEST:"
        Print "Source: $($environmentInfo.PullRequestSource)"
        Print "Hash: $($environmentInfo.PullRequestCommitHash)"
    }
}

$rootPath = Convert-Path "$($scriptsPath)\..\"

ArchiveArtifactsIfAny($rootPath)

& $scriptsPath\Build.ps1 -configuration Debug

$version = "0.0.0-local-$([DateTime]::UtcNow.ToString('yyyyMMdd-HHmmss'))"

if ($environmentInfo.IsServer) {
    if ($environmentInfo.IsTagged) {
        $version = $environmentInfo.TagVersion
    }
    elseif ($environmentInfo.IsCommit) {
        $version = "0.0.0-commit-$($environmentInfo.CommitHash.Substring(0, 7))"
    }
}

Print "Version: $($version)"

$isUbuntuOrLocal = $environmentInfo.IsUbuntuServer -or $environmentInfo.IsLocal

& $scriptsPath\RunUnitTests.ps1 -build $false -checkCoverage $isUbuntuOrLocal -coverageReportTag $version -generateReport $environmentInfo.IsLocal

& $scriptsPath\RunFunctionalTests.ps1 -build $false

if ($isUbuntuOrLocal) {
    & $scriptsPath\CreatePackage.ps1 -version $version -build $true
}

if ($environmentInfo.IsUbuntuServer) {

    $artifactsPath = Convert-Path "$($rootPath)\artifacts"

    $publishCoverageReportConditions = @(
        [pscustomobject]@{name = "Ubuntu CI"; checked = $environmentInfo.IsUbuntuServer},
        [pscustomobject]@{name = "Not PR"; checked = $environmentInfo.IsPullRequest -eq $false},
        [pscustomobject]@{name = "Master branch"; checked = $environmentInfo.IsMasterBranch},
        [pscustomobject]@{name = "Coveralls api keys"; checked = $environmentInfo.ContainsCoverallsApiKey}
    )

    $publishCoverageReportConditionsResult = CheckAllConditions ($publishCoverageReportConditions)

    Print "Publishing coverage report to coveralls.io conditions:"
    Print $publishCoverageReportConditionsResult.log

    if ($publishCoverageReportConditionsResult.passed) {
        $coverageReportPath = Convert-Path "$($artifactsPath)\coverage\coverage.opencover.xml"

        & $scriptsPath\PublishCoverageReport.ps1 -path $coverageReportPath -coverallsApiKey $environmentInfo.CoverallsApiKey
    }

    $publishNugetPackageConditions = @(
        [pscustomobject]@{name = "Ubuntu CI"; checked = $environmentInfo.IsUbuntuServer},
        [pscustomobject]@{name = "Tag"; checked = $environmentInfo.IsTagged},
        [pscustomobject]@{name = "Not PR"; checked = $environmentInfo.IsPullRequest -eq $false},
        [pscustomobject]@{name = "Master branch"; checked = $environmentInfo.IsMasterBranch},
        [pscustomobject]@{name = "NuGet api keys"; checked = $environmentInfo.ContainsNugetApiKey}
    )

    $publishNugetPackageConditionsResult = CheckAllConditions ($publishNugetPackageConditions)


    Print "Publishing nuget package to nuget.org conditions:"
    Print $publishNugetPackageConditionsResult.log

    if ($publishNugetPackageConditionsResult.passed) {
        $nugetPackagePath = Convert-Path "$($artifactsPath)\nuget\CoreValidation.$($version).nupkg"

        & $scriptsPath\PublishPackage.ps1 -path $nugetPackagePath -nugetApiKey $environmentInfo.NugetApiKey
    }
}


Print "DONE! :)"