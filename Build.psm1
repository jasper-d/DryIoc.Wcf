function HashToRevision([string] $hash){
    $longHash = [Int32]::Parse($hash.Substring(0,4), [System.Globalization.NumberStyles]::HexNumber)
    #AssemblyVersionAttribute does not support revisions greater than 2**16 - 2
    if($longHash -le 65534) {
        return $longHash
    } else {
        return [Int32]::Parse($hash.Substring(0,3), [System.Globalization.NumberStyles]::HexNumber)
    }
}

function TestVersionTag([string] $tag) {
    $tag -Match '^v\.\d\.\d{1,2}\.\d{1,3}$'
}

function SetVersion([string] $projectFile, [string] $newVersion){
    Write-Host "Setting csproj version"
    [xml]$xml = Get-Content $projectFile
    $propertyGroups = $xml.Project.PropertyGroup

    foreach ($pg in $xml.Project.PropertyGroup | Where-Object {$_.Version -ne $null -and $_.PackageId -ne $Null}) {
        $pg.Version = $newVersion
    }
    $xml.Save($projectFile)
}

function SetReleaseVariable($newVersion){
    Write-Host "Updating Build variables"
    Set-AppveyorBuildVariable -Name "DeployArtifacts" -Value "true"
    Set-AppveyorBuildVariable -Name "ReleaseVersion" -Value $newVersion
}

function PrepareRelease([string] $projectFile){
    $commitHashInt = HashToRevision $env:APPVEYOR_REPO_COMMIT

    Write-Host "Preparing release"
    Write-Host "APPVEYOR_REPO_TAG: $($env:APPVEYOR_REPO_TAG)"
    Write-Host "APPVEYOR_REPO_TAG_NAME: $($env:APPVEYOR_REPO_TAG_NAME)"

    if ($env:APPVEYOR_REPO_TAG -eq "true" -and (TestVersionTag $env:APPVEYOR_REPO_TAG_NAME)) {
        $newVersion = "$($env:APPVEYOR_REPO_TAG_NAME.TrimStart("v.")).$commitHashInt"
        Write-Host "New release $($newVersion)"
        Update-AppveyorBuild -Version $newVersion
        SetVersion $projectFile $newVersion
        SetReleaseVariable $newVersion
    }
    else
    {
        Update-AppveyorBuild -Version "$($env:APPVEYOR_BUILD_VERSION).$commitHashInt"
    }
}

function Set-ReleaseInformation($projectFile){
    PrepareRelease $projectFile
}

Export-ModuleMember -Function  *-*