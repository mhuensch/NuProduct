using NuGet;
using System.IO;

namespace Run00.NuProductWindowsConsole
{
	public class NuGetFactory : INuGetFactory
	{
		public NuGetFactory(Arguments arguments)
		{
			_arguments = arguments;
		}

		NuGet.IPackageManager INuGetFactory.GetPackageManager()
		{
			if (_packageManager != null)
				return _packageManager;

			var repo = ((INuGetFactory)this).GetPackageRepository();
			return new PackageManager(repo, _arguments.InstallationDirectory);
		}

		NuGet.IPackageRepository INuGetFactory.GetPackageRepository()
		{
			if (_packageRepository != null)
				return _packageRepository;

			var buildSource = Path.GetDirectoryName(_arguments.TargetPackage);
			var packageSources = new[] { buildSource, _arguments.NugetHost };
			return new AggregateRepository(PackageRepositoryFactory.Default, packageSources, ignoreFailingRepositories: true);
		}

		private IPackageManager _packageManager;
		private IPackageRepository _packageRepository;
		private readonly Arguments _arguments;
	}
}
