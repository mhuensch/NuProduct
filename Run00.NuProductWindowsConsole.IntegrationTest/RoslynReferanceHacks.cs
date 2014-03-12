using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using Roslyn.Services.CSharp.Classification;
using Run00.NuProductVersioning;

namespace Run00.NuProductWindowsConsole.IntegrationTest
{
	internal static class RoslynReferanceHacks
	{
		private static void Include()
		{
			//The following are hacks to get the integration test to include these dlls
			typeof(SemanticVersioning).ToString();
			typeof(NuProductRoslyn.PackageReader).ToString();
			typeof(NuProductCecil.PackageReader).ToString();
			typeof(Workspace).ToString();
			typeof(AliasSymbol).ToString();
			typeof(SyntaxClassifier).ToString();
		}
	}
}
