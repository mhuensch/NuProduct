param($installPath, $toolsPath, $package, $project)
$text = (gc $toolsPath\Run00.NuProductWindowsConsoleAdditional.props.template).replace("[path]","$($toolsPath)\Run00.NuProductWindowsConsole.exe")
$text | Set-Content "$($installPath)\build\Run00.NuProductWindowsConsoleAdditional.props"
