Run00.NuProduct
======

Build utility used to calculate the semantic version based on the current build and the latest NuGet package based on:
http://semver.org/

Note: To rebuild nuspec files for integration testing, the file "PackSamples.bat" must be run.  Each sample project includes
a .nuprod file that will be copied to a matching .nuspec before a pack command is executed.  This is to prevent the build
server from trying to pack and push these sample projects as NuGet packages.
