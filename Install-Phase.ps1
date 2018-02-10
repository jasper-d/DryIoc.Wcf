Import-Module $PSScriptRoot\Scripts.psm1

$projectFile = (Resolve-Path .\DryIoc.Wcf\DryIoc.Wcf.csproj)
Set-ReleaseInformation($projectFile)
