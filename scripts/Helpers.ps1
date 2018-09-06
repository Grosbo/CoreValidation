$ErrorActionPreference = 'Stop'

function Exec {
    [CmdletBinding()]
    param(
        [Parameter(Position = 0, Mandatory = 1)][string]$message,
        [Parameter(Position = 1, Mandatory = 1)][scriptblock]$cmd
    )

    Print $message

    & $cmd

    if ($lastexitcode -ne 0) {
        throw ("ERROR when >>$($message)<<")
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

function ArchiveArtifactsIfAny {
    param(
        [Parameter(Position = 0, Mandatory = 1)][string]$rootPath
    )

    $artifactsDirItem = Get-ChildItem -Path $rootPath -Filter "artifacts" -Directory

    if ($null -ne $artifactsDirItem) {
        New-Item -ItemType Directory -Force -Path "$($rootPath)\artifacts_archive"
        Move-Item -Path "$($rootPath)\artifacts" -Destination "$($rootPath)\artifacts_archive\$($artifactsDirItem.CreationTime.ToString('yyyyMMdd-HHmmss'))"
    }
}

function CheckAllConditions {
    param([pscustomobject[]]$checks)

    $log = ""
    $passed = $true

    for ($i = 0; $i -lt $checks.length; $i++) {
        $log = ( $log + "`n$($checks[$i].name): $($checks[$i].checked)")
        if ($passed -and ($checks[$i].checked -eq $false)) {
            $passed = $false
        }
    }

    $log = $log + "`nRESULT: $($passed)"

    return New-Object PsObject -Property @{log = $log ; passed = $passed}
}

function TryGetToolExecutableItem {
    param([string]$toolsDir,
        [string]$version,
        [string]$executable)

    return Get-ChildItem -Path $toolsDir -Filter $executable -Recurse -ErrorAction SilentlyContinue -Force |
        Where-Object {$_.FullName.Split([System.IO.Path]::DirectorySeparatorChar).Contains($version)} |
        Select-Object -First 1
}
