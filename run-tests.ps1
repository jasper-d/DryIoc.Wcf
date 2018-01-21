$port = 11119
$iisExpressPath = "c:\program files\iis express\iisexpress.exe"
If ([environment]::Is64BitOperatingSystem) {
    $iisExpressPath = "c:\program files (x86)\iis express\iisexpress.exe"
}
$appPath = (Resolve-path .\WcfSample.Service)
$testAppPath = (Resolve-path .\IntegrationTests\bin\Release\IntegrationTests.dll)

$iisExpressSettings = @"
IIS Express:
iisExpressPath:  $iisExpressPath
port:            $port
appPath:         $appPath
testAppPath:     $testAppPath

"@

Write-Host $iisExpressSettings  -ForegroundColor Magenta
Write-Host "Starting IIS Express"  -ForegroundColor Magenta

Start-Process -FilePath $iisExpressPath -ArgumentList "/port:$port /path:$appPath"  -WindowStyle Hidden

Write-Host "Executing test..." -ForegroundColor Magenta

packages\xunit.runner.console.2.2.0-beta4-build3444\tools\xunit.console $testAppPath -xml .\test-results.xml

$xunitExitCode = $LastExitCode

#Upload test results when running on Appveyor
if($env:APPVEYOR_JOB_ID) {
	Write-Host "Uploading test results..."  -ForegroundColor Magenta
    $wc = New-Object 'System.Net.WebClient'
    $wc.UploadFile("https://ci.appveyor.com/api/testresults/xunit/$($env:APPVEYOR_JOB_ID)", (Resolve-Path .\test-results.xml))
}

Write-Host "Stop IIS Express"  -ForegroundColor Magenta

Stop-Process -Name "iisexpress"

#Terminate build if tests fail
if($xunitExitCode -ne 0) {
	Write-Host "Test failed, terminating build" -ForegroundColor Yellow -BackgroundColor Red
	$host.SetShouldExit($xunitExitCode)
}