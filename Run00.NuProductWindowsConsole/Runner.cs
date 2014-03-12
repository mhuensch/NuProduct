using Run00.NuProduct;
using Run00.NuProductCecil;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Run00.NuProductWindowsConsole
{
	public class Runner : IRunner
	{
		public Runner(ISemanticVersioning versioning, INuGet nuget, NuGetConfiguration config, IEnumerable<IPackageReader> readers)
		{
			_versioning = versioning;
			_nuget = nuget;
			_config = config;
			_readers = readers;
		}

		VersionChange IRunner.Execute(string[] args)
		{
			var solutionFile = new FileInfo(args.First()).FullName;
			var packageId = args.Skip(1).Take(1).Single();

			var neoReader = _readers.Where(r => r.CanReadPackage(solutionFile, packageId)).FirstOrDefault();
			var package = neoReader.ReadPackage(solutionFile, packageId);

			////TODO: add parsing for more complex arguments
			//var path = args.First();
			//_config.PackageSource = args.Skip(1).Take(1).SingleOrDefault();
			//_config.InstallPath = args.Skip(2).Take(1).SingleOrDefault();

			//var manifestStream = default(Stream);
			//var filePath = Guid.NewGuid().ToString();
			//using (var stream = File.OpenRead(path))
			//{
			//	var archive = new System.IO.Compression.ZipArchive(stream);
			//	var nuspecFile = archive.Entries.Where(e => Path.GetExtension(e.FullName) == ".nuspec").SingleOrDefault();
			//	if (nuspecFile == null)
			//		throw new InvalidOperationException("The supplied nuget package did not contain a nuspec file");

			//	manifestStream = nuspecFile.Open();
			//	using (var write = File.OpenWrite(filePath))
			//	{
			//		manifestStream.CopyTo(write);
			//		write.Flush();
			//	}
			//}


			//var manifest = _nuget.ReadManifest(filePath);
			//var packageId = manifest.Metadata.Id;

			//var change = _versioning.Calculate(null, packageId);

			//var semanticVersion = new SemanticVersion(change.New);
			//manifest.Metadata.Version = semanticVersion.ToString();

			//using (var write = File.OpenWrite(filePath))
			//{
			//	manifest.Save(write);
			//}

			return null;
		}

		private readonly ISemanticVersioning _versioning;
		private readonly INuGet _nuget;
		private readonly NuGetConfiguration _config;
		private readonly IEnumerable<IPackageReader> _readers;
	}
}
