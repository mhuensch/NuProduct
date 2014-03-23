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

			_nugetFactory.UpdateTargetManifest(newVersion.ToString());

			//TODO: Clean up old package and install directory

			_nugetFactory.GetPackageManager().UninstallPackage(targetPackage, true, true);
			_nugetFactory.GetPackageManager().UninstallPackage(publishedPackage, true, true);

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

		private readonly ISemanticVersioning _versioning;
		private readonly IPackageReader _packageReader;
		private readonly INuGetFactory _nugetFactory;
		private readonly IArguments _arguments;
	}
}
