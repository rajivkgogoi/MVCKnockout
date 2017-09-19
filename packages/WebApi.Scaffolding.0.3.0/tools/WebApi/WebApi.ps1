[T4Scaffolding.Scaffolder(Description = "This Scaffolder create a WCF Web API service")][CmdletBinding()]
param(        
    [parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ApiName,
    [parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ResourceName,
    [string]$DbContextType,
    [string]$Project,
    [string]$CodeLanguage,
    [string[]]$TemplateFolders,
    [switch]$Force = $false,
	[switch]$Repository = $false,
    [switch]$UseIoC = $false
)


$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

Scaffold WebApiResource -ResourceName $ResourceName

if ($Repository) {
	if(!$DbContextType) { $DbContextType = [System.Text.RegularExpressions.Regex]::Replace((Get-Project $Project).Name, "[^a-zA-Z0-9]", "") + "Context" }
	Scaffold Repository -ModelType $ResourceName -DbContextType $DbContextType -Project $Project -CodeLanguage $CodeLanguage -Force:$Force -BlockUi
}

$route = Get-PluralizedWord $ResourceName.Replace("Resource", "").ToLower()
$templateName = if($Repository) { "WebApiWithRepositoryTemplate" } else { "WebApiTemplate" }
Add-ProjectItemViaTemplate "Api\$ApiName" -Template $templateName `
	-Model @{ 
		Namespace = $namespace; 
		ApiName = $ApiName; 
		ResourceName = $ResourceName;
		Route = $route;
		UseIoC = [Boolean]$UseIoC;
	} -SuccessMessage "Added WebApi output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force

$appStartName = "App_Start\$ApiName" + "Start"
Add-ProjectItemViaTemplate $appStartName -Template WebApiAppStartTemplate `
	-Model @{ 
		Namespace = $namespace; 
		ApiName = $ApiName; 
		Route = $route;
		UseIoC = [Boolean]$UseIoC;
	} -SuccessMessage "Added WebApi output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force

if ($UseIoC) {
#	Add-ProjectItemViaTemplate "DependencyResolution\IoCResourceFactory" -Template WebApiIoCResourceFactoryTemplate `
#		-Model @{ 
#			Namespace = $namespace; 
#		} -SuccessMessage "Added WebApi output at {0}" `
#		-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force

	Add-ProjectItemViaTemplate "DependencyResolution\IoC" -Template WebApiIoCTemplate `
		-Model @{ 
			Namespace = $namespace; 
		} -SuccessMessage "Added WebApi output at {0}" `
		-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force
}
