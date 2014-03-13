using NuGet;

namespace Run00.NuProduct
{
	//TODO: Rewrite this so that Nuget.Core is not exposed through public contracts
	public interface INuGetFactory
	{
		IPackageManager GetPackageManager();

		IPackageRepository GetPackageRepository();

		Manifest GetTargetManifest();

		void UpdateTargetManifest(string version);
	}
}
