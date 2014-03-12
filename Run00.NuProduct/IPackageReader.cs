using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.NuProduct
{
	public interface IPackageReader
	{
		bool CanReadPackage(string path, string projectId);

		PackageDefinition ReadPackage(string path, string projectId);
	}
}
