Include -fileNamePathToInclude .\functions.ps1

Properties {
	$build_dir =  Split-Path $psake.build_script_file
	$targetpath = Split-Path $build_dir -Parent
	$build_artifacts_dir = "$targetpath\buildartifacts"
	$sln_file =  ls $targetpath -Recurse "LicensePrototypeSolution.sln"
	$SourceDir = "$targetpath\src"

	$WebProject = ls "$targetpath\src\"+$ProjectName "*.csproj"
	#$WebProject = ls "$targetpath\src\License.MetCalWeb" "*.csproj"
    $IISAppNode = "parameters.setParameter"
    $SiteName = $env:IISAppName
	#$SiteName = "WebApp"
	$Package = "$SiteName.zip"
    $PkgLocation = $build_artifacts_dir+"\"+$Package
    $Destination=$env:Destination
	$UserName = $env:DeploymentUserName
	$Pwd = $env:DeploymentUserPassword
	#$Destination="localhost"
	#$UserName = "pshivaru"
	#$Pwd = "Qwerty#9876"
	$configuration = $env:BuildConfiguration
	#$configuration = "Debug"
	$SitefinityUrl = $env:WebUrl
	#$SitefinityUrl = "http://localhost/WebApp"
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

task -name Clean -action {
    Remove-Item $build_artifacts_dir -Recurse -Force -Confirm:$false
    Write-Host "All Artifacts have been cleaned." -ForegroundColor Green
} -precondition{
    Test-Path $build_artifacts_dir
}

task -name Init -depends Clean -action{
    ni -ItemType Directory $build_artifacts_dir
    Write-Host "New Artifact directory has been created." -ForegroundColor Green
}

task -name PackageRestore -action{
	Write-Host $sln_file.FullName
	Exec {..\.nuget\NuGet.exe restore $sln_file.FullName}
}

task -name Compile -depends Init, PackageRestore -action {
	if($sln_file -ne $null)
	{
		Write-Host "file present" -ForegroundColor Green
		Build-Solution -SlnFile $sln_file.FullName -BuildAction Build -Configuration $configuration
	}
	else
	{
		Assert -failureMessage "Solution file not present"
		throw "Couldn't find Solution file"
	}
}

task -name Package -depends Compile -action{
    Get-Location | Write-Host -ForegroundColor Yellow
    Write-Host $PkgLocation -BackgroundColor DarkGreen

    Package-Project -ProjectFile $WebProject.FullName -Configuration $configuration -PackageLocation $PkgLocation | Write-Host -ForegroundColor Cyan
} -precondition{
    if($WebProject.Exists -eq $false){
        Write-Host "Web Project file not found" -ForegroundColor Red
        }
    return $WebProject.Exists
}

task -name ChangeConfigs -depends Package -action{
        pushd $build_artifacts_dir
        $IISConfig = ls *.SetParameters.xml -Recurse -File
        popd
        Change-ConfigParam -XMLFile $IISConfig.FullName -NodePath $IISAppNode -NewValue $SiteName
        Write-Host "IIS COnfig has been updated." -ForegroundColor Green
    }

task -name Deploy -depends ChangeConfigs -action{
    $pkg = '"'+$PkgLocation+'"'

    $DeployParamFile = ls $build_artifacts_dir *.SetParameters.xml

	try{
	    Write-Host "Starting deployment site $SiteName to $Destination" -ForegroundColor Cyan
		Write-Host $DeployParamFile
	    Deploy-Package -Destination $Destination -PackageFile $pkg -IISAppName $SiteName -UserName $UserName -Password $Pwd -MSDeployParamFile $DeployParamFile
	}

	catch{
		$ErrorMessage = $_.Exception.Message
        throw "$ErrorMessage"
	}
}

task default -depends Deploy