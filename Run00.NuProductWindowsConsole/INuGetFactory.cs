
using NuGet;
namespace Run00.NuProductWindowsConsole
{
	public interface INuGetFactory
	{
		IPackageManager GetPackageManager();

		IPackageRepository GetPackageRepository();
	}
}
