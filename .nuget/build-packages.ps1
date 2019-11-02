
Add-Type -assembly "system.io.compression.filesystem"

$moduleName = "EPiServer.DeveloperTools"
$source = $PSScriptRoot + "\..\DeveloperTools\modules\_protected\" + $moduleName
$destination = $PSScriptRoot + "\..\DeveloperTools\" + $moduleName + ".zip"

If(Test-path $destination) {Remove-item $destination}
[io.compression.zipfile]::CreateFromDirectory($Source, $destination)

.\nuget.exe pack ..\DeveloperTools\DeveloperTools.csproj -Properties Configuration=Release

