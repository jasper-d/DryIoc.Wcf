function HashToRevision([string] $hash){
    $longHash = [Int32]::Parse($hash.Substring(0,4), [System.Globalization.NumberStyles]::HexNumber)
    #AssemblyVersionAttribute not support revisions greater than 16**2 - 2
    if($longHash -le 65534) {
        return $longHash
    } else {
        return [Int32]::Parse($hash.Substring(0,3), [System.Globalization.NumberStyles]::HexNumber)
    }
}

function TestVersionTag([string] $tag) {
    $tag -match '^v\.\d\.\d{1,2}\.\d{1,3}$'
}

function SetVersion([string] $projectFile, [string] $newVersion){
    [xml]$xml = Get-Content $projectFile
    $propertyGroups = $xml.Project.PropertyGroup

    foreach ($pg in $xml.Project.PropertyGroup | Where-Object {$_.Version -ne $null -and $_.PackageId -ne $Null}) {
        $pg.Version = $newVersion
    }
    $xml.Save($projectFile)
}

function SetReleaseVariable($newVersion){
    Set-AppveyorBuildVariable -Name "DeployArtifacts" -Value "true"
    Set-AppveyorBuildVariable -Name "ReleaseVersion" -Value $newVersion
}

function PrepareRelease([string] $projectFile){
    $commitHashInt = HashToRevision $env:APPVEYOR_REPO_COMMIT

    if ($env:APPVEYOR_REPO_TAG -eq "true" -and (TestVersionTag $env:APPVEYOR_REPO_TAG_NAME)) {
        $newVersion = "$($env:APPVEYOR_REPO_TAG_NAME.TrimStart("v.")).$commitHashInt"
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