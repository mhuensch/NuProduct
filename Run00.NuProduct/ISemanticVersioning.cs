
namespace Run00.NuProduct
{
	public interface ISemanticVersioning
	{
		VersionChange Calculate(PackageDefinition neoPackage, PackageDefinition paleoPackage);
	}
}
