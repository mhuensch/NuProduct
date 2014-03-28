using Ionic.Zip;
using NuGet;
using Run00.NuProduct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

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


			//TODO: read product id and version to store and update
			var productFile = targetPackage
				.GetFiles()
				.Where(f => Path.GetExtension(f.Path).Equals(".nuprod", StringComparison.InvariantCultureIgnoreCase))
				.FirstOrDefault();

			var newVersion = default(SemanticVersion);
			var change = default(VersionChange);
			if (productFile != null)
			{
				var packageDir = _nugetFactory.GetPackageManager().PathResolver.GetPackageDirectory(targetPackage);
				var path = Path.Combine(_arguments.GetInstallationDirectory(), packageDir, productFile.Path);
				var serializer = new XmlSerializer(typeof(Product));
				var product = default(Product);
				using (var stream = File.OpenRead(path))
				{
					product = (Product)serializer.Deserialize(stream);
				}

				using (var zip = ZipFile.Read(product.CopyVersionFrom))
				{
					var entry = zip.Where(e => Path.GetExtension(e.FileName).Equals(".nuspec")).FirstOrDefault();
					var m = Manifest.ReadFrom(entry.OpenReader(), true);
					change = new VersionChange() { Change = new Version(m.Metadata.Version) };
					newVersion = new SemanticVersion(change.Change);
				}
			}
			else
			{
				change = _versioning.Calculate(targetDeffinition, publishedDeffinition);
				var major = publishedPackage.Version.Version.Major + change.Change.Major;
				var minor = publishedPackage.Version.Version.Minor + change.Change.Minor;
				var patch = publishedPackage.Version.Version.Build + change.Change.Build;
				newVersion = new SemanticVersion(major, minor, patch, string.Empty);
			}

			_nugetFactory.UpdateTargetManifest(newVersion.ToString());

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
