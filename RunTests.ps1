# run tests
dotnet test "--logger:trx;LogFileName=results.trx"

# upload results to AppVeyor
$files = Get-ChildItem -Path ".\" -Recurse -Include *.trx

$files | Foreach-Object {
	$wc = New-Object 'System.Net.WebClient'
	$wc.UploadFile("https://ci.appveyor.com/api/testresults/mstest/$($env:APPVEYOR_JOB_ID)", $_.FullName)
}