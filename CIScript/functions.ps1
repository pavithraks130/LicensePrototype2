function Build-Solution
{
	param(

		[Parameter(Mandatory=$True)]$SlnFile,
		[Parameter(Mandatory=$False)]
		[ValidateSet("Build","Clean","Rebuild")]$BuildAction = "Build",
        [Parameter(Mandatory=$False)]
        [ValidateSet("Debug","Release")]$Configuration = "Debug"
	)

	Process
	{
        Write-Host "Building Solution..." -ForegroundColor Cyan
		msbuild $SlnFile /t:$BuildAction /p:Configuration=$Configuration /p:VisualStudioVersion=10.0
        Write-Host "Solution Has been built" -ForegroundColor Green
	}
}

function Package-Project
{
    param(

		[Parameter(Mandatory=$True)]$ProjectFile,
		[Parameter(Mandatory=$True)]$PackageLocation,
        [Parameter(Mandatory=$False)]
        [ValidateSet("Debug","Release")]$Configuration = "Release"
	)

    Process
    {
		
        Write-Host "Packaging the project..." -ForegroundColor Cyan
        msbuild $ProjectFile /t:Package /p:Configuration=$Configuration /p:PackageLocation=$PackageLocation /p:VisualStudioVersion=10.0
        Write-Host "Project has been packaged and ready to deploy."
        return $PackageLocation
    }
}

function Package-FromProfile
{
    param(

		[Parameter(Mandatory=$True)]$SlnFile,
		[Parameter(Mandatory=$True)]$PackageLocation,
        [Parameter(Mandatory=$True)]
		[string]$PublishProfile,
		[Parameter(Mandatory=$False)]
		[bool]$DeployOnBuild = $True
	)

    Process
    {
        Write-Host "Packaging the project..." -ForegroundColor Cyan
        msbuild $SlnFile /p:DeployOnBuild=true /p:PublishProfile=$PublishProfile /p:PackageLocation=$PackageLocation /p:VisualStudioVersion=10.0
        Write-Host "Project has been packaged and ready to deploy."
        return $PackageLocation
    }
}

function Change-ConfigParam
{
    Param(

        [Parameter(Mandatory=$True)]
        [string]$XMLFilePath,
        [Parameter(Mandatory=$True)]
        [string]$NodePath,
        [Parameter(Mandatory=$True)]
        [string]$NewValue,
        [Parameter(Mandatory=$false)]
        [string]$Attribute
    )

    Process{
        $path = $XMLFilePath
        $xml = [xml](cat $path)
        $childNode = Get-XmlNode -XmlDocument $xml -NodePath $NodePath
        $childNode.value = $NewValue
        $xml.Save($path)
    }
}

function Get-XmlNode
{
    param(
        [Parameter(Mandatory=$True)]
        [xml]$XmlDocument,
        [Parameter(Mandatory=$True)]
        [string]$NodePath,
        [Parameter(Mandatory=$False)]
        [string]$NamespaceURI = "",
        [Parameter(Mandatory=$False)]
        [string]$NodeSeparatorCharacter = '.'
    )

    # If a Namespace URI was not given, use the Xml document's default namespace.
    if ([string]::IsNullOrEmpty($NamespaceURI)) { $NamespaceURI = $XmlDocument.DocumentElement.NamespaceURI }

    # In order for SelectSingleNode() to actually work, we need to use the fully qualified node path along with an Xml Namespace Manager, so set them up.
    $xmlNsManager = New-Object System.Xml.XmlNamespaceManager($XmlDocument.NameTable)
    $xmlNsManager.AddNamespace("ns", $NamespaceURI)
    $fullyQualifiedNodePath = "/ns:$($NodePath.Replace($($NodeSeparatorCharacter), '/ns:'))"

    # Try and get the node, then return it. Returns $null if the node was not found.
    $node = $XmlDocument.SelectSingleNode($fullyQualifiedNodePath, $xmlNsManager)
    return $node
}

function Deploy-Package
{
    Param(
        [Parameter(Mandatory=$True)]
        [string]$Destination,
        [Parameter(Mandatory=$True)]
        [string]$PackageFile,
        [Parameter(Mandatory=$True)]
        [string]$IISAppName,
        [Parameter(Mandatory=$True)]
        [string]$UserName,
        [Parameter(Mandatory=$True)]
        [string]$Password,
        [Parameter(Mandatory=$True)]
        [string]$MSDeployParamFile,
        [Parameter(Mandatory=$False)]
        [string]$MSDeployParams = "-verb:sync -allowUntrusted -disableLink:AppPoolExtension -disableLink:ContentExtension -disableLink:CertificateExtension",
        [Parameter(Mandatory=$False)]
        [string]$WhatIf = ""
    )

    Process{
        $serverUrl = "https://${Destination}:8172/msdeploy.axd"
	    $dest = "auto,computerName=$serverUrl,username=$UserName,password=$Password,authtype=Basic,includeAcls=False"
        $configParam = "-setParam:Name='IIS Web Application Name',Value='$IISAppName'"
        $Deployment_Command = "msdeploy.exe -verb:sync -source:package=$PackageFile -dest:auto  $configParam"

        try{
			Write-Host $Deployment_Command
			cmd.exe /C "$Deployment_Command"
        }

        catch{
            $ErrorMessage = $_.Exception;
            throw "Error Deploying Package : $ErrorMessage"
        }
    }
}

