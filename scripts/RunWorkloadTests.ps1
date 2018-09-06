Param (
    [Parameter(Position = 0, Mandatory = $true)][string]$baseUrl,
    [Parameter(Position = 1, Mandatory = $true)][int]$durationInSeconds,
    [Parameter(Position = 2, Mandatory = $false)][int]$threads = 8
)

$ErrorActionPreference = 'Stop'

$scriptsPath = Convert-Path $PSScriptRoot
. $scriptsPath\Helpers.ps1

$rootPath = Convert-Path "$($scriptsPath)\..\"
$testsPath = Convert-Path "$($rootPath)\test"

$testCases = @(
    [pscustomobject]@{
        path    = "api/login";
        body    = "{'Email':'bartosz', 'Password':'lenar'}";
        sleep   = 100;
        isError = $true;
    },
    [pscustomobject]@{
        path    = "api/signup";
        body    = "{'Email':'bartosz@test.com', 'Password':'zaq12wsxc'}";
        sleep   = 50;
        isError = $true;
    }
)

$startDate = [DateTime]::Now
$counter = -1
do {
    $running = @(Get-Job | Where-Object { ($_.State -eq 'Running') })

    if ($running.Count -lt $threads) {

        $counter = $counter + 1

        $testCase = $testCases[$counter % $testCases.Length]

        Start-Job {
            $base = $args[0];
            $case = $args[1];
            Start-Sleep -Milliseconds $case.sleep

            try {
                Invoke-RestMethod -Method 'Post' -Uri "$($base)/$($case.path)" -Body $case.body -ContentType 'application/json'

                if ($case.isError) {
                    throw ("ERROR! Should result with error")
                }
            }
            catch {
                if ($case.isError -eq $false) {
                    throw ("Should not result with error")
                }
            }

        } -ArgumentList $($baseUrl, $testCase)

    }
    else {
        $running | Wait-Job
    }
    Get-Job | Receive-Job | Out-Null
} while ([DateTime]::Now.Subtract($startDate).TotalSeconds -le $durationInSeconds)

