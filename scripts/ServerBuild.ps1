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

& $scriptsPath\Build.ps1
& $scriptsPath\RunUnitTests.ps1 -build $false
& $scriptsPath\RunFunctionalTests.ps1 -build $false

if ($environmentInfo.IsTagged) {
    $version = $environmentInfo.TagVersion
}
else {
    $envPostfix = & {If ($environmentInfo.IsUbuntuServer) {"ubuntu"} Else {"windows"}}

    if ($environmentInfo.IsCommit) {
        $version = "0.0.0-commit-$($environmentInfo.CommitHash.Substring(0, 7))-$($envPostfix)"
    }
    else {
        $version = "0.0.0-time-$([DateTime]::UtcNow.ToString('yyyyMMddHHmmss'))-$($envPostfix)"
    }
}

& $scriptsPath\CreatePackage.ps1 -version $version -build $false

$publichChecks = @(
    [pscustomobject]@{name = "Tag"; checked = $environmentInfo.IsTagged},
    [pscustomobject]@{name = "Not PR"; checked = $environmentInfo.IsPullRequest -eq $false},
    [pscustomobject]@{name = "Master branch"; checked = $environmentInfo.IsMasterBranch},
    [pscustomobject]@{name = "Ubuntu server"; checked = $environmentInfo.IsUbuntuServer},
    [pscustomobject]@{name = "NuGet api keys"; checked = $environmentInfo.ContainsNugetApiKey}
)

$publishNugetPackage = $true

Print "PUBLISH CHECKS:"
for ($i = 0; $i -lt $publichChecks.length; $i++) {
    Print "$($publichChecks[$i].name): $($publichChecks[$i].checked)"

    if ($publishNugetPackage -and ($publichChecks[$i].checked -eq $false)) {
        $publishNugetPackage = $false
    }
}

Print "ALL CHECKED: $($publishNugetPackage)"

if ($publishNugetPackage) {
    $nugetPackagePath = Convert-Path "$($scriptsPath)\..\artifacts\CoreValidation.$($version).nupkg"

    & $scriptsPath\PublishPackage.ps1 -path $nugetPackagePath -nugetApiKey "asd"
}

Print "DONE! :)"