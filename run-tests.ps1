if(!(Test-Path .\test-results.xml)){
    New-Item -name test-results.xml
}

$opencover = (Resolve-Path .\packages\OpenCover.*\tools\OpenCover.Console.exe)
$coveralls = (Resolve-Path .\packages\coveralls.io.*\tools\coveralls.net.exe)
$xunit = (Resolve-Path .\packages\xunit.runner.console.*\tools\net4*\xunit.console.exe)
$testResults = (Resolve-Path .\test-results.xml)

$port = 11119
$iisExpressPath = "c:\program files\iis express\iisexpress.exe"
If ([environment]::Is64BitOperatingSystem) {
    $iisExpressPath = "c:\program files (x86)\iis express\iisexpress.exe"
}
$appPath = (Resolve-path .\WcfSample.Service)
$appPathBin = (Resolve-path .\WcfSample.Service\bin)
$unitTestAppPath = (Resolve-path .\DryIoc.Wcf.Tests\bin\Release\DryIoc.Wcf.Tests.dll)
$integrationTestAppPath = (Resolve-path .\IntegrationTests\bin\Release\IntegrationTests.dll)

$iisExpressSettings = @"
IIS Express settings:
iisExpressPath:  	$iisExpressPath
port:            	$port
appPath:         	$appPath
integrationTestAppPath: $integrationTestAppPath
"@

Write-Host $iisExpressSettings  -ForegroundColor Magenta
Write-Host "Starting IIS Express"  -ForegroundColor Magenta

Start-Process -FilePath $iisExpressPath -ArgumentList "/port:$port /path:$appPath"  -WindowStyle Hidden

Write-Host "Executing test..." -ForegroundColor Magenta

&$xunit $unitTestAppPath $integrationTestAppPath -xml $testResults

$xunitExitCode = $LastExitCode

write-host "stop iis express"  -foregroundcolor magenta
stop-process -name "iisexpress"

#Upload test results when running on Appveyor
if($env:APPVEYOR_JOB_ID) {
	Write-Host "Uploading test results..."  -ForegroundColor Magenta
    $wc = New-Object 'System.Net.WebClient'
    $wc.UploadFile("https://ci.appveyor.com/api/testresults/xunit/$($env:APPVEYOR_JOB_ID)", $testResults)
}

Remove-Item $testResults -ErrorAction SilentlyContinue

#Terminate build if tests fail
if($xunitExitCode -ne 0) {
    Write-Host "Test failed, terminating build" -ForegroundColor Yellow -BackgroundColor Red
    $host.SetShouldExit($xunitExitCode)
}

#Run OpenCover

&$opencover -searchdirs:"$integrationTestAppPath" -register:user -filter:"+[DryIoc.Wcf]* -[DryIoc.Wcf.Test]*" -target:"$xunit" -targetargs:"$unitTestAppPath -noshadow -nologo" -output:coverage.xml

$args = "-log:All -register:user -targetdir:`"$appPathBin`" -output:coverage.xml -filter:`"+[DryIoc.Wcf]* -[DryIoc.Wcf.Test]*`" -target:`"$iisExpressPath`" -targetargs:`"/port:$port /path:$appPath /systray:false`" -mergebyhash -mergeoutput"

Start-Process $opencover -ArgumentList $args -NoNewWindow
Start-Sleep -s 5

&$xunit $integrationTestAppPath
stop-process -name "iisexpress"
start-sleep -s 20

if($LastExitCode -ne 0) {
    Write-Host "Coverage analysis failed, terminating build" -ForegroundColor Yellow -BackgroundColor Red
    $host.SetShouldExit($LastExitCode)
} 

write-host "Done testing"
