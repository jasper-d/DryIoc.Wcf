$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
Write-Host $root
$version = [System.Reflection.Assembly]::LoadFile("$root\DryIoc.Wcf\bin\Release\DryIoc.Wcf.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\nuget\DryIoc.Wcf.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\nuget\DryIoc.Wcf.compiled.nuspec

& nuget pack $root\nuget\DryIoc.Wcf.nuspec -Version $version -Symbols -OutputDirectory "$root" -NonInteractive -Build -Properties Configuration=Release

#nuget pack 