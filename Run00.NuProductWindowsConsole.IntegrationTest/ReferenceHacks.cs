using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.NuProductWindowsConsole.IntegrationTest
{
	internal static class ReferenceHacks
	{
		private static void Include()
		{
			//These hacks exist because MsTest does not load these dlls into the test bin unless directly referenced
			typeof(NuProductCecil.PackageReader).ToString();
			typeof(NuProductVersioning.SemanticVersioning).ToString();
		}
	}
}
