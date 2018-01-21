$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'

Write-Host "Root path: $root" -ForegroundColor Magenta
Write-Host "Getting version from assembly..." -ForegroundColor Magenta

$version = [System.Reflection.Assembly]::LoadFile("$root\DryIoc.Wcf\bin\Release\DryIoc.Wcf.dll").GetName().Version

Write-Host "Version:   $version" -ForegroundColor Magenta
Write-Host "Packing NuGet packages..." -ForegroundColor Magenta
nuget pack $root\nuget\DryIoc.Wcf.nuspec -Version $version -Symbols -OutputDirectory "$root\nuget\artifacts" -NonInteractive