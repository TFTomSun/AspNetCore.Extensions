param([String]$scriptFileName, [String]$arguments) #Must be the first statement in your script

$url = 'https://raw.githubusercontent.com/TFTomSun/Environment.GitHub/master/AppVeyor/' + $scriptFileName;
# Download the script
Invoke-WebRequest -Uri $url -OutFile $scriptFileName
# Invoke the script
Invoke-Expression ".\$scriptFileName $arguments"

#$Script = Invoke-WebRequest $url;
#$ScriptBlock = [Scriptblock]::Create($Script.Content)
#Invoke-Command -ScriptBlock $ScriptBlock -ArgumentList ($args + @('someargument'))
