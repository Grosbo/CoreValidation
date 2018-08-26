Param (
    [Parameter(Position = 0, Mandatory = $true)][string]$path,
    [Parameter(Position = 1, Mandatory = $true)][string]$coverallsApiKey
)

$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

$rootPath = Convert-Path "$($scriptsPath)\..\"
$toolsPath = "$($rootPath)\tools"

New-Item -ItemType Directory -Force -Path $toolsPath
$toolsPath = Convert-Path $toolsPath

$coverallsPublisherToolItem = TryGetToolExecutableItem $toolsPath "coveralls.net" "1.0.0" "csmacnz.Coveralls.dll"

if (($null -eq $executableItem)) {
    & dotnet tool install "coveralls.net" --tool-path $toolsPath --version "1.0.0"
    $coverallsPublisherToolItem = TryGetToolExecutableItem $toolsPath "coveralls.net" "1.0.0" "csmacnz.Coveralls.dll"
}

Exec "Publishing code coverage report: $($path)" {
  & dotnet ($coverallsPublisherToolItem.FullName) --opencover -i $path --repoToken $coverallsApiKey --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE --jobId $env:APPVEYOR_JOB_ID
}
