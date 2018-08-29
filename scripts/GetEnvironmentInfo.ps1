return @{
    "IsLocal"                 = -not (Test-Path env:APPVEYOR)
    "IsServer"                = (Test-Path env:APPVEYOR) -and ($env:APPVEYOR.ToLower() -eq "true")
    "IsUbuntuServer"          = ($env:APPVEYOR_BUILD_WORKER_IMAGE -eq "Ubuntu")
    "IsCommit"                = (Test-Path env:APPVEYOR_REPO_COMMIT)
    "CommitHash"              = $env:APPVEYOR_REPO_COMMIT
    "CommitDate"              = $env:APPVEYOR_REPO_COMMIT_TIMESTAMP
    "CommitAuthor"            = $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL
    "CommitMessage"           = $env:APPVEYOR_REPO_COMMIT_MESSAGE
    "IsTagged"                = (Test-Path env:APPVEYOR_REPO_TAG_NAME) -and ($env:APPVEYOR_REPO_TAG_NAME -ne "")
    "TagName"                 = $env:APPVEYOR_REPO_TAG_NAME
    "TagVersion"              = if (Test-Path env:APPVEYOR_REPO_TAG_NAME) { $env:APPVEYOR_REPO_TAG_NAME.TrimStart("v") } else { "" }
    "IsPullRequest"           = (Test-Path env:APPVEYOR_PULL_REQUEST_NUMBER)
    "PullRequestSource"       = $env:APPVEYOR_PULL_REQUEST_HEAD_REPO_NAME
    "PullRequestCommitHash"   = $env:APPVEYOR_PULL_REQUEST_HEAD_COMMIT
    "Branch"                  = $env:APPVEYOR_REPO_BRANCH
    "IsMasterBranch"          = $env:APPVEYOR_REPO_BRANCH -eq "master"
    "ContainsNugetApiKey"     = (Test-Path env:NUGET_API_KEY) -and ($env:NUGET_API_KEY -ne "")
    "NugetApiKey"             = $env:NUGET_API_KEY
    "ContainsCoverallsApiKey" = (Test-Path env:COVERALLS_API_KEY) -and ($env:COVERALLS_API_KEY -ne "")
    "CoverallsApiKey"         = $env:COVERALLS_API_KEY
}