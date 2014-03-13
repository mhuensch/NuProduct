cd ..
for /f %%d in ('dir /od /a-d /b /s Test.Sample.*.nuprod') do copy %%d %%~dpd\%%~nd.nuspec
for /f %%d in ('dir /od /a-d /b /s Test.Sample.*.csproj') do .nuget\Nuget.exe pack %%d -OutputDirectory Run00.NuProductWindowsConsole.IntegrationTest\Artifacts
.nuget\Nuget.exe pack Test.Sample.ControlGroup\Test.Sample.ControlGroup.csproj -OutputDirectory Run00.NuProductWindowsConsole.IntegrationTest\Artifacts -Version 1.0.0
cd .nuget
