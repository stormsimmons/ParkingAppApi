param([string] $TestProjectDirectory,
[string] $ApiName,		
[string] $ProtocolName,
[string] $DNS)

Write-Host "Running Api test"	

$ProjSettingPath = $TestProjectDirectory + "\project.settings"

try
{
	$raw = Get-Content $ProjSettingPath -raw
	$contentSettings = ConvertFrom-Json $raw
	
	$contentSettings.variables.api = $ApiName 
	$contentSettings.variables.protocol = $ProtocolName 
	$contentSettings.variables.dns = $DNS 
	
	$contentSettings | ConvertTo-Json | Set-Content $ProjSettingPath	

	C:\BuildTools\Telerik\TestStudio\Bin\ApiTesting\runnerconsole\Telerik.ApiTesting.Runner.exe test -p $TestProjectDirectory 
	 	
	Write-Host "Finished running api test"
    exit 0
}
catch 
{
    Write-Error $_
    exit 1
}	

	


	