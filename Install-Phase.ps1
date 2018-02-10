Import-Module $PSScriptRoot\Build.psm1

$projectFile = (Resolve-Path .\DryIoc.Wcf\DryIoc.Wcf.csproj)
Set-ReleaseInformation($projectFile)
