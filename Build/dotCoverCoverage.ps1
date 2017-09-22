param([string] $TestAssembliesDirectory,		
		[string] $TestResultsDirectory,
		  [string] $TestCategories)
		
Write-Host "Coverage with dotCover"	

$excludedAssemblies = ""
$hasTestRunFailed = 0

try {

$testDlls = Get-ChildItem $TestAssembliesDirectory -File -include "*.dll" -Recurse| Where-Object { $_.Name -like "*Test*.dll" } | Select-Object
	
	foreach ($testDll in $testDlls)
	{ 
		$dotCoverTargetArguments += @($testDll.FullName)	
	}	
	 	
	$excludedAssemblies = $excludedAssemblies + "+:module=*;class=*;function=*;-:module=*Test*;"
	if ($TestCategories)
	{
		$testCatsString = $TestCategories -replace ',', '|TestCategory='
		$dotCoverTargetArguments +="/TestCaseFilter:`"TestCategory=$testCatsString`""
	}
	
	c:\BuildTools\dotCover\dotcover.exe cover /TargetExecutable="c:\BuildTools\TestWindow\vstest.console.exe" /Filters="$excludedAssemblies" /TargetWorkingDir="$TestAssembliesDirectory" /TargetArguments="$dotCoverTargetArguments" /Output="$TestResultsDirectory\CoverageReport.dcvr" /LogFile="$TestResultsDirectory\DotCoverlog.txt"

    $hasTestRunFailed = $LASTEXITCODE
    	
    c:\BuildTools\dotCover\dotcover.exe report /Source="$TestResultsDirectory\CoverageReport.dcvr" /Output="$TestResultsDirectory\CoverageReport.html" /ReportType="HTML"
    c:\BuildTools\dotCover\dotcover.exe report /Source="$TestResultsDirectory\CoverageReport.dcvr" /Output="$TestResultsDirectory\CoverageReport.xml" /ReportType="XML"
 
    Write-Host "Finished running dotCover"
	
	 exit 0
    }
    catch {
        Write-Error $_
        exit 1
    }	

	