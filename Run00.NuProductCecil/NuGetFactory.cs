using Ionic.Zip;
using NuGet;
using Run00.NuProduct;
using System;
using System.IO;
using System.Linq;

namespace Run00.NuProductCecil
{
	public class NuGetFactory : INuGetFactory
	{
		public NuGetFactory(IArguments arguments)
		{
			_arguments = arguments;
		}

		NuGet.IPackageManager INuGetFactory.GetPackageManager()
		{
			if (_packageManager != null)
				return _packageManager;

			var repo = ((INuGetFactory)this).GetPackageRepository();
			_packageManager = new PackageManager(repo, _arguments.GetInstallationDirectory());
			return _packageManager;
		}

		NuGet.IPackageRepository INuGetFactory.GetPackageRepository()
		{
			if (_packageRepository != null)
				return _packageRepository;

			var buildSource = Path.GetDirectoryName(_arguments.GetTargetPackagePath());
			var packageSources = new[] { buildSource, _arguments.GetNugetHost() };
			_packageRepository = new AggregateRepository(PackageRepositoryFactory.Default, packageSources, ignoreFailingRepositories: true);
			return _packageRepository;
		}

		Manifest INuGetFactory.GetTargetManifest()
		{
			var result = default(Manifest);
			using (var zip = ZipFile.Read(_arguments.GetTargetPackagePath()))
			{
				var entry = zip.Where(e => Path.GetExtension(e.FileName).Equals(".nuspec")).FirstOrDefault();
				result = Manifest.ReadFrom(entry.OpenReader(), true);
			}
			return result;
		}

		void INuGetFactory.UpdateTargetManifest(string version)
		{
			var result = default(Manifest);
			using (var zip = ZipFile.Read(_arguments.GetTargetPackagePath()))
			{
				var entry = zip.Where(e => Path.GetExtension(e.FileName).Equals(".nuspec")).FirstOrDefault();
				result = Manifest.ReadFrom(entry.OpenReader(), true);
				result.Metadata.Version = version;

				var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
				Directory.CreateDirectory(dir);
				var path = Path.Combine(dir, entry.FileName);
				using (var write = File.Open(path, FileMode.Create))
				{
					result.Save(write);
				}
				zip.UpdateItem(path, string.Empty);
				zip.Save();
			}
		}

		private IPackageManager _packageManager;
		private IPackageRepository _packageRepository;
		private readonly IArguments _arguments;

	}
}
