$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
Write-Host $root
$version = [System.Reflection.Assembly]::LoadFile("$root\DryIoc.Wcf\bin\Release\DryIoc.Wcf.dll").GetName().Version

& nuget pack $root\nuget\DryIoc.Wcf.nuspec -Version $version -Symbols -OutputDirectory "$root" -NonInteractive -Build -Properties Configuration=Release