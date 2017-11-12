git config --global credential.helper store
git config --global user.email "no@mail.com"
git config --global user.name "AppVeyor Agent"

Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:github_access_token):x-oauth-basic@github.com`n"
Get-Content "$env:USERPROFILE\.git-credentials"
echo "APPVEYOR_PROJECT_ID $env:APPVEYOR_PROJECT_ID"
echo "APPVEYOR_PROJECT_NAME $env:APPVEYOR_PROJECT_NAME"
echo "APPVEYOR_ACCOUNT_NAME $env:APPVEYOR_ACCOUNT_NAME"
echo "APPVEYOR_PROJECT_SLUG $env:APPVEYOR_PROJECT_SLUG"
echo "APPVEYOR_PROJECT_SLUG $env:APPVEYOR_REPO_BRANCH"

$readMeContentFileName = "README.content.md"
echo "read $readMeContentFileName" 
if([System.IO.File]::Exists($readMeContentFileName)){
	$readMe = Get-Content $readMeContentFileName
}
else{
	echo "no $readMeContentFileName file found." 
	$readMe = ""
}
      
echo "read build status header file"     
$header = ((Invoke-webrequest -URI "https://raw.githubusercontent.com/TFTomSun/Environment.GitHub/master/BuildStatus.template.md").Content).replace("[APPVEYOR_REPO_NAME]",$($env:APPVEYOR_REPO_NAME)).replace("[APPVEYOR_PROJECT_NAME]", $($env:APPVEYOR_PROJECT_NAME)).replace("[APPVEYOR_BUILD_VERSION]", $($env:APPVEYOR_BUILD_VERSION)).replace("[APPVEYOR_PROJECT_SLUG]", $($env:APPVEYOR_PROJECT_SLUG)).replace("[APPVEYOR_ACCOUNT_NAME]", $($env:APPVEYOR_ACCOUNT_NAME))

echo "Build status header is : $header"
Set-Content README.md –value $header, $readMe

git add -A
git commit -m "[skip ci] README.md updated by AppVeyor build"
git push origin HEAD:$($env:APPVEYOR_REPO_BRANCH)