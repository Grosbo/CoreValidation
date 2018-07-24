Param (
    [string]$v = "",
    [string]$k = ""
)
function Exec {
  [CmdletBinding()]
  param(
      [Parameter(Position = 0, Mandatory = 1)][scriptblock]$cmd,
      [Parameter(Position = 1, Mandatory = 0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
  )
  & $cmd
  if ($lastexitcode -ne 0) {
      throw ("ERROR: " + $errorMessage)
  }
}

function Print {
  if ($args) {
      $storedColor = $host.UI.RawUI.ForegroundColor
      $host.UI.RawUI.ForegroundColor = "Blue"
      Write-Output ">>> $($args)"
      $host.UI.RawUI.ForegroundColor = $storedColor
  }
}


function Build {
    Param (
        [string]$version = "",
        [string]$nugetApiKey = ""
    )

    & Print "Cleaning artifacts"
    if (Test-Path .\artifacts) {
        Remove-Item .\artifacts -Force -Recurse
    }

    & Print "dotnet SDK"
    Exec { & dotnet --version }

    & Print "Cleaning builds"
    Exec { & dotnet clean -c Release  --verbosity m }

    & Print "Building"
    Exec { & dotnet build .\CoreValidation.sln -c Release --no-incremental }

    & Print "Unit tests"
    Exec { & dotnet test .\tests\CoreValidation.UnitTests\CoreValidation.UnitTests.csproj -c Release --no-build }

    & Print "Unit tests (predefined rules)"
    Exec { & dotnet test .\tests\CoreValidation.PredefinedRules.UnitTests\CoreValidation.PredefinedRules.UnitTests.csproj -c Release --no-build }

    & Print "Functional tests"
    Exec { & dotnet test .\tests\CoreValidation.FunctionalTests\CoreValidation.FunctionalTests.csproj -c Release --no-build }

    if ($version -ne "") {
        & Print "Packing version: $($version)"
        Exec { & dotnet pack .\src\CoreValidation\CoreValidation.csproj -c Release --no-build -o .\..\..\artifacts --verbosity q /p:Version=$version }

        if ($nugetApiKey -ne "") {
            & Print "Publishing: $($version)"

            Exec {
                $nugetPackagePath = Convert-Path ".\artifacts\CoreValidation.$($version).nupkg";
                & dotnet nuget push $nugetPackagePath -k $nugetApiKey -s https://api.nuget.org/v3/index.json
            }
        }
    }
}

if ((-not (Test-Path env:APPVEYOR)) -or ($env:APPVEYOR.ToLower() -ne "true") ) {
    & Print "APPVEYOR environment not detected!"
    & Build -version $v -nugetApiKey $k
}
else {

    $isPullRequest = $false

    & Print "Image: $($env:APPVEYOR_BUILD_WORKER_IMAGE)"

    if (Test-Path env:APPVEYOR_PULL_REQUEST_NUMBER) {
        $isPullRequest = $true
        & Print "Pull request: $($env:APPVEYOR_PULL_REQUEST_TITLE) #$($env:APPVEYOR_PULL_REQUEST_NUMBER)"
        & Print "From: $($env:APPVEYOR_PULL_REQUEST_HEAD_REPO_NAME) / $($env:APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH)"
        & Print "Head Commit: $($env:APPVEYOR_PULL_REQUEST_HEAD_COMMIT)"
    }

    if (Test-Path env:APPVEYOR_REPO_COMMIT) {
        if (Test-Path env:APPVEYOR_REPO_TAG_NAME) {
            & Print "Tag: $($env:APPVEYOR_REPO_TAG_NAME)"
        }
        & Print "Commit: $($env:APPVEYOR_REPO_COMMIT)"
        & Print "Date: $($env:APPVEYOR_REPO_COMMIT_TIMESTAMP)"
        & Print "Author: $($env:APPVEYOR_REPO_COMMIT_AUTHOR) <$($env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL)>"
        & Print "Message: $($env:APPVEYOR_REPO_COMMIT_MESSAGE)"
        & Print "$($env:APPVEYOR_REPO_COMMIT_MESSAGE_EXTENDED)"
    }
    else {
        & Print "NON-COMMIT"
    }

    if (($isPullRequest -eq $false) -and
        ($env:APPVEYOR_BUILD_WORKER_IMAGE -eq "Ubuntu") -and
        (Test-Path env:APPVEYOR_REPO_TAG_NAME) -and ($env:APPVEYOR_REPO_TAG_NAME -ne "") -and
        (Test-Path env:NUGET_API_KEY) -and ($env:NUGET_API_KEY -ne "")) {
        $version = "$($env:APPVEYOR_REPO_TAG_NAME.TrimStart("v"))";

        & Print "Building and publishing $($version)"
        & Build -version $version -nugetApiKey $env:NUGET_API_KEY

    }
    else {
        & Print "Just building..."
        & Build
    }
}



