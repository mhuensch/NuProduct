using NuGet;

namespace Run00.NuProduct
{
	public interface INuGetFactory
	{
		IPackageManager GetPackageManager();

		IPackageRepository GetPackageRepository();
	}
}
