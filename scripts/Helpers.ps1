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

