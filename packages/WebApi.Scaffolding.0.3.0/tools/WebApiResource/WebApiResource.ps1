[T4Scaffolding.Scaffolder(Description = "This Scaffolder create a WCF Web API service")][CmdletBinding()]
param(        
    [parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ResourceName,
    [string]$Project,
    [string]$CodeLanguage,
    [string[]]$TemplateFolders,
    [switch]$Force = $false
)


$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

Add-ProjectItemViaTemplate "Resources\$ResourceName" -Template WebApiResourceTemplate `
	-Model @{ Namespace = $namespace; ResourceName = $ResourceName } `
	-SuccessMessage "Added WebApi output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force

