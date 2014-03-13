using NuGet;
using Run00.NuProduct;
using System.IO;

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
			return new PackageManager(repo, _arguments.GetInstallationDirectory());
		}

		NuGet.IPackageRepository INuGetFactory.GetPackageRepository()
		{
			if (_packageRepository != null)
				return _packageRepository;

			var buildSource = Path.GetDirectoryName(_arguments.GetTargetPackage());
			var packageSources = new[] { buildSource, _arguments.GetNugetHost() };
			return new AggregateRepository(PackageRepositoryFactory.Default, packageSources, ignoreFailingRepositories: true);
		}

		private IPackageManager _packageManager;
		private IPackageRepository _packageRepository;
		private readonly IArguments _arguments;
	}
}
