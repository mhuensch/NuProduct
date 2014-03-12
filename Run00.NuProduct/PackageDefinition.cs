using System;
using System.Collections.Generic;

namespace Run00.NuProduct
{
	public class PackageDefinition
	{
		public Version Version { get; set; }

		public ICollection<string> MemberKeys { get; set; }
	}
}
