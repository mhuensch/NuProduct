using CommandLine;
using CommandLine.Text;
using System.IO;

namespace Run00.NuProduct
{
	public class Arguments : IArguments
	{
		[Option("target", Required = true,
		 HelpText = "Nuget package file to be versioned")]
		public string TargetPackage { get; set; }

		[Option("host", Required = true, DefaultValue = "https://nuget.org/api/v2/",
		 HelpText = "Host address for the Nuget repository where the latest package can be downloaded")]
		public string NugetHost { get; set; }

		[Option("out", Required = false,
		 HelpText = "The output dir for the revised nupkg (defaults to the supplied target directory)")]
		public string OutputDirectory { get; set; }

		[Option("install", Required = false,
		 HelpText = "Installation directory for the latest package (defaults to the NuProduct folder in the user's temp dir)")]
		public string InstallationDirectory { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}

		string IArguments.GetTargetPackagePath()
		{
			return TargetPackage;
		}

		string IArguments.GetNugetHost()
		{
			return NugetHost;
		}

		string IArguments.GetOutputDirectory()
		{
			if (string.IsNullOrEmpty(OutputDirectory))
				OutputDirectory = Path.GetDirectoryName(TargetPackage);

			return OutputDirectory;
		}

		string IArguments.GetInstallationDirectory()
		{
			if (string.IsNullOrEmpty(InstallationDirectory))
				InstallationDirectory = GetProductDir();

			return InstallationDirectory;
		}

		public static string GetProductDir()
		{
			return Path.Combine(Path.GetTempPath(), _productDir);
		}

		private const string _productDir = "NuProduct";
	}
}
