using CommandLine;
using CommandLine.Text;

namespace Run00.NuProduct
{
	public class Arguments : IArguments
	{
		[Option("target", Required = true,
		 HelpText = "Nuget package file to be versioned")]
		public string TargetPackage { get; set; }

		[Option("version", Required = false, DefaultValue = "0.0.0-autoversion",
		 HelpText = "Target version used when looking for local packages")]
		public string TargetVersion { get; set; }

		[Option("host", Required = false, DefaultValue = "https://nuget.org/api/v2/",
		 HelpText = "Host address for the Nuget repository where the latest package can be downloaded")]
		public string NugetHost { get; set; }

		[Option("out", Required = false,
		 HelpText = "Installation directory for the latest package")]
		public string InstallationDirectory { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}

		string IArguments.GetTargetPackage()
		{
			return TargetPackage;
		}

		string IArguments.GetTargetVersion()
		{
			return TargetVersion;
		}

		string IArguments.GetNugetHost()
		{
			return NugetHost;
		}

		string IArguments.GetInstallationDirectory()
		{
			return InstallationDirectory;
		}
	}
}
