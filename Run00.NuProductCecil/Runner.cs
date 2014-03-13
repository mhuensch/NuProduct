using NuGet;
using Run00.NuProduct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Run00.NuProductCecil
{
	public class Runner : IRunner
	{
		public Runner(ISemanticVersioning versioning, IPackageReader reader, INuGetFactory nugetFactory, IArguments arguments)
		{
			_versioning = versioning;
			_nugetFactory = nugetFactory;
			_packageReader = reader;
			_arguments = arguments;
		}

		VersionChange IRunner.Execute()
		{
			var repo = _nugetFactory.GetPackageRepository();
			var manifest = _nugetFactory.GetTargetManifest();

			var targetPackage = repo.FindPackage(manifest.Metadata.Id, new SemanticVersion(manifest.Metadata.Version));
			var publishedPackage = repo.FindPackage(manifest.Metadata.Id);

			if (targetPackage == null || publishedPackage == null)
				throw new InvalidOperationException("Can not read package " + manifest.Metadata.Id);

			var targetDeffinition = GetPackageDefinition(targetPackage, true);
			var publishedDeffinition = GetPackageDefinition(publishedPackage, false);

			var change = _versioning.Calculate(targetDeffinition, publishedDeffinition);

			var major = publishedPackage.Version.Version.Major + change.Change.Major;
			var minor = publishedPackage.Version.Version.Minor + change.Change.Minor;
			var patch = publishedPackage.Version.Version.Build + change.Change.Build;
			var newVersion = new SemanticVersion(major, minor, patch, string.Empty);

			UpdatePackage(targetPackage, newVersion);

			//TODO: Update the current nuget package with the new version (File Name and Manifest)
			//TODO: Clean up old package and install directory

			return change;
		}

		public IEnumerable<string> GetPackageDefinition(IPackage package, bool includePreRelease)
		{
			_nugetFactory.GetPackageManager().InstallPackage(package, true, includePreRelease);

			var packageDir = _nugetFactory.GetPackageManager().PathResolver.GetPackageDirectory(package);
			var dllFiles = package.GetLibFiles().Select(f => Path.Combine(_arguments.GetInstallationDirectory(), packageDir, f.Path));

			var result = _packageReader.ReadPackage(dllFiles);
			return result;
		}

		private void UpdatePackage(IPackage targetPackage, SemanticVersion newVersion)
		{
			//TODO: add parsing for more complex arguments
			//TODO: get package install directory without copy paste

			_nugetFactory.UpdateTargetManifest(newVersion.ToString());


			//var packageDir = _nugetFactory.GetPackageManager().PathResolver.GetPackageDirectory(targetPackage);
			//var fullDir = Path.Combine(_arguments.GetInstallationDirectory(), packageDir);
			//var unzipedDir = Path.Combine(Path.GetTempPath(), "unziped");
			//var nupkgPath = Path.Combine(fullDir, packageDir + ".nupkg");

			//if (Directory.Exists(unzipedDir))
			//	Directory.Delete(unzipedDir, true);

			//ZipFile.ExtractToDirectory(nupkgPath, unzipedDir);

			//var manifestPath = Directory.GetFiles(unzipedDir, "*.nuspec").FirstOrDefault();
			//var manifest = default(Manifest);

			//using (var stream = File.OpenRead(manifestPath))
			//{
			//	manifest = Manifest.ReadFrom(stream, true);
			//}

			//using (var stream = File.Open(manifestPath, FileMode.Create))
			//{
			//	manifest.Metadata.Version = newVersion.ToString();
			//	manifest.Save(stream);
			//}

			//using (var zip = Ionic.Zip.ZipFile.Read(nupkgPath))
			//{
			//	zip.RemoveEntry(Path.GetFileName(manifestPath));
			//	zip.AddFile(manifestPath, string.Empty);
			//	zip.Save();
			//}


			//var newPath = Path.Combine(Path.GetDirectoryName(_arguments.GetTargetPackageId()), targetPackage.GetFullName().Split(' ').First() + "." + newVersion + ".nupkg");
			//File.Move(nupkgPath, newPath);

			//using (var modFile = ZipFile.Open(nupkgPath, ZipArchiveMode.Update))
			//{
			//	modFile.CreateEntryFromFile(manifestPath, Path.GetFileName(manifestPath), CompressionLevel.Optimal);
			//}
			//File.Copy(manifestPath, @"C:\Test.nuspec", true);


			//using (var stream = File.OpenRead(nupkgPath))
			//{
			//	var archive = new ZipArchive(stream);
			//	var nuspecStream = archive.Entries.Where(e => Path.GetExtension(e.FullName) == ".nuspec").SingleOrDefault();
			//	if (nuspecStream == null)
			//		throw new InvalidOperationException("The supplied nuget package did not contain a nuspec file");

			//	using (var manifestZipStream = nuspecStream.Open())
			//	{
			//		using (var manifestStream = File.OpenWrite(manifestPath))
			//		{
			//			manifestZipStream.CopyTo(manifestStream);
			//			var manifest = Manifest.ReadFrom(manifestStream, true);
			//			manifest.Metadata.Version = newVersion.ToString();
			//			manifestStream.Flush();
			//		}
			//		//TODO: add manifest file to zip stream
			//		//TODO: rename package file
			//	}
			//}
		}

		private readonly ISemanticVersioning _versioning;
		private readonly IPackageReader _packageReader;
		private readonly INuGetFactory _nugetFactory;
		private readonly IArguments _arguments;
	}
}
