
namespace Run00.NuProduct
{
	public interface IArguments
	{
		string GetTargetPackage();

		string GetTargetVersion();

		string GetNugetHost();

		string GetInstallationDirectory();
	}
}
