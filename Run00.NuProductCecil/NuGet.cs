using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Run00.NuProductCecil
{
	public class NuGet : INuGet
	{
		public NuGet(NuGetConfiguration config)
		{
			_config = config;
		}

		IPackage INuGet.GetLatestPackage(string packageId)
		{
			return GetPackageRepository().GetPackages()
				.Where(p => string.Compare(p.Id, packageId, StringComparison.InvariantCultureIgnoreCase) == 0)
				.OrderBy(p => p.Version)
				.LastOrDefault();
		}

		IEnumerable<string> INuGet.GetAssemblies(IPackage package)
		{
			if (package == null)
				return Enumerable.Empty<string>();

			var dir = GetPackageManager().PathResolver.GetPackageDirectory(package);

			if (GetPackageManager().FileSystem.DirectoryExists(dir) == false)
				GetPackageManager().InstallPackage(package, true, false);

			var fullDir = GetPackageManager().FileSystem.GetFullPath(dir);
			var assemblies = package.AssemblyReferences.Select(a => Path.Combine(fullDir, a.Path));
			return assemblies;
		}

		Manifest INuGet.ReadManifest(string path)
		{
			var result = default(Manifest);
			using (var stream = File.OpenRead(path))
			{
				result = Manifest.ReadFrom(stream, true);
			}
			return result;
		}

		void INuGet.SavePackageVersion(string path, Version version)
		{
			var manifest = Manifest.ReadFrom(null, false);



			manifest.Save(null);
		}

		private IPackageManager GetPackageManager()
		{
			if (_packageManager == null)
				_packageManager = new PackageManager(GetPackageRepository(), _config.InstallPath);
			return _packageManager;
		}

		private IPackageRepository GetPackageRepository()
		{
			if (_packageRepository == null)
				_packageRepository = PackageRepositoryFactory.Default.CreateRepository(_config.PackageSource);
			return _packageRepository;
		}

		private readonly NuGetConfiguration _config;
		private IPackageManager _packageManager;
		private IPackageRepository _packageRepository;

	}
}
