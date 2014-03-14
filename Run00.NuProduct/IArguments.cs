
namespace Run00.NuProduct
{
	public interface IArguments
	{
		string GetTargetPackagePath();

		string GetNugetHost();

		string GetOutputDirectory();

		string GetInstallationDirectory();
	}
}
