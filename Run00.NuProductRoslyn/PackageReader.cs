using Roslyn.Services;
using Run00.NuProduct;
using System;
using System.IO;
using System.Linq;

namespace Run00.NuProductRoslyn
{
	public class PackageReader : IPackageReader
	{
		bool IPackageReader.CanReadPackage(string path, string projectId)
		{
			if (File.Exists(path) == false)
				return false;

			if (Path.GetExtension(path).Equals(".sln") == false)
				return false;

			var result = GetProject(path, projectId) != null;
			return result;
		}

		PackageDefinition IPackageReader.ReadPackage(string path, string projectId)
		{
			var project = GetProject(path, projectId);
			if (project == null)
				throw new InvalidOperationException("Can not read project from solution");

			var comp = (ICompilation)new RoslynCompilation(project.GetCompilation());
			var example = comp.ToFullString();

			return new PackageDefinition()
			{
				MemberKeys = null,
				Version = new Version(comp.GetVersion())
			};
		}

		private IProject GetProject(string path, string projectId)
		{
			var filePath = new FileInfo(path).FullName;
			var solution = Solution.Load(filePath);
			var project = solution.Projects.Where(p => p.Name.Equals(projectId)).FirstOrDefault();
			return project;
		}
	}
}
