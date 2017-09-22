param(
[string] $JMeterLoadTest,	
[string] $ApiName,		
[string] $ProtocolName,
[string] $DNS)
		
Write-Host "Running JMeter Load Test"	

try
{
	$xml = [xml](Get-Content $JMeterLoadTest)

    $node = $xml.jmeterTestPlan.hashTree.TestPlan.elementProp.collectionProp.ChildNodes

    foreach($elementProp in $node)
    {
        if ($elementProp.name -eq "ApiName") 
        {
         
            $stringProps = $elementProp.stringProp

            foreach($stringProp in $stringProps)
            {
                if ($stringProp.name -eq "Argument.value") 
                {
                    $stringProp.'#text'  =  $ApiName   
                }
            }
        }

        if ($elementProp.name -eq "Protocol") 
        {
            $stringProps = $elementProp.stringProp

            foreach($stringProp in $stringProps)
            {
                if ($stringProp.name -eq "Argument.value") 
                {         
                    $stringProp.'#text'  =  $ProtocolName   
                }
            }
        }

        if ($elementProp.name -eq "DNS") 
        {
            $stringProps = $elementProp.stringProp

            foreach($stringProp in $stringProps)
            {
                if ($stringProp.name -eq "Argument.value") 
                {         
                    $stringProp.'#text'  =  $DNS   
                }
            }
           
        }
    }
    
	
	 $xml.Save($JMeterLoadTest)
	
	cmd.exe /c "C:\BuildTools\apache-jmeter\bin\jmeter.bat" -n -t $JMeterLoadTest 

	Write-Host "Finished JMeter Load Test"
	
    exit 0
}
catch 
{
    Write-Error $_
    exit 1
}	
	