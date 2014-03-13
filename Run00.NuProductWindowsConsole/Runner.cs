using NuGet;
using Run00.NuProduct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Run00.NuProductWindowsConsole
{
	public class Runner : IRunner
	{
		public Runner(ISemanticVersioning versioning, IPackageReader reader, INuGetFactory nugetFactory, Arguments arguments)
		{
			_versioning = versioning;
			_nugetFactory = nugetFactory;
			_packageReader = reader;
			_arguments = arguments;
		}

		VersionChange IRunner.Execute()
		{
			var targetDeffinition = GetPackageDefinition(_arguments.TargetPackage, new SemanticVersion(_arguments.TargetVersion), true);
			var publicDeffinition = GetPackageDefinition(_arguments.TargetPackage, null, false);

			var change = _versioning.Calculate(targetDeffinition, publicDeffinition);
			return change;


			//TODO: Update the current nuget package with the new version (File Name and Manifest)
			//TODO: Clean up old package and install directory
		}

		public IEnumerable<string> GetPackageDefinition(string packageId, SemanticVersion version, bool includePreRelease)
		{
			var repo = _nugetFactory.GetPackageRepository();
			var package = default(IPackage);

			if (version == null)
				package = repo.FindPackage(packageId);
			else
				package = repo.FindPackage(packageId, version);

			if (package == null)
				throw new InvalidOperationException("Can not read package " + packageId);

			_nugetFactory.GetPackageManager().InstallPackage(package, true, includePreRelease);

			var packageDir = _nugetFactory.GetPackageManager().PathResolver.GetPackageDirectory(package);
			var dllFiles = package.GetLibFiles().Select(f => Path.Combine(_arguments.InstallationDirectory, packageDir, f.Path));

			var result = _packageReader.ReadPackage(dllFiles);
			return result;
		}

		private readonly ISemanticVersioning _versioning;
		private readonly IPackageReader _packageReader;
		private readonly INuGetFactory _nugetFactory;
		private readonly Arguments _arguments;
	}
}




//var neoReader = _readers.Where(r => r.CanReadPackage(solutionFile, packageId)).FirstOrDefault();
//var package = neoReader.ReadPackage(solutionFile, packageId);

////TODO: add parsing for more complex arguments
//var path = args.First();
//_config.PackageSource = args.Skip(1).Take(1).SingleOrDefault();
//_config.InstallPath = args.Skip(2).Take(1).SingleOrDefault();

//var manifestStream = default(Stream);
//var filePath = Guid.NewGuid().ToString();
//using (var stream = File.OpenRead(path))
//{
//	var archive = new System.IO.Compression.ZipArchive(stream);
//	var nuspecFile = archive.Entries.Where(e => Path.GetExtension(e.FullName) == ".nuspec").SingleOrDefault();
//	if (nuspecFile == null)
//		throw new InvalidOperationException("The supplied nuget package did not contain a nuspec file");

//	manifestStream = nuspecFile.Open();
//	using (var write = File.OpenWrite(filePath))
//	{
//		manifestStream.CopyTo(write);
//		write.Flush();
//	}
//}


//var manifest = _nuget.ReadManifest(filePath);
//var packageId = manifest.Metadata.Id;

//var change = _versioning.Calculate(null, packageId);

//var semanticVersion = new SemanticVersion(change.New);
//manifest.Metadata.Version = semanticVersion.ToString();

//using (var write = File.OpenWrite(filePath))
//{
//	manifest.Save(write);
//}

