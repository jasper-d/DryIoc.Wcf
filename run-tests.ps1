$port = 11119
$iisExpressPath = "c:\program files\iis express\iisexpress.exe"
If ([environment]::Is64BitOperatingSystem) {
    $iisExpressPath = "c:\program files (x86)\iis express\iisexpress.exe"
}
$appPath = (Resolve-path .\WcfSample.Service)
$testAppPath = (Resolve-path .\IntegrationTests\bin\Debug\IntegrationTests.dll)
#$testResultsPath = (Resolve-path .\test-results.xml)
Write-Host "IIS Express:"
Write-Host "iisExpressPath:  " $iisExpressPath
Write-Host "port:            " $port
Write-Host "appPath:         " $appPath
Write-Host "testAppPath:     " $testAppPath


Start-Process -FilePath $iisExpressPath -ArgumentList "/port:$port /path:$appPath"  -WindowStyle Hidden

Write-host "Executing test..."

packages\xunit.runner.console.2.2.0-beta4-build3444\tools\xunit.console $testAppPath -xml .\test-results.xml

if($env:APPVEYOR_JOB_ID) {
    $wc = New-Object 'System.Net.WebClient'
    $wc.UploadFile("https://ci.appveyor.com/api/testresults/xunit/$($env:APPVEYOR_JOB_ID)", (Resolve-Path .\test-results.xml))
}

Write-Host "Stop IIS Express"

Stop-Process -Name "iisexpress"