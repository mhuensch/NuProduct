
namespace Run00.NuProductWindowsConsole.IntegrationTest
{
	internal static class ReferenceHacks
	{
		private static void Include()
		{
			//These hacks exist because MsTest does not load these dlls into the test bin unless directly referenced
			typeof(NuProductCecil.PackageReader).ToString();
		}
	}
}
