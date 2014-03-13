using Mono.Cecil;
using NuGet;
using Run00.NuProduct;
using System.Collections.Generic;
using System.Linq;

namespace Run00.NuProductCecil
{
	public class PackageReader : IPackageReader
	{
		IEnumerable<string> IPackageReader.ReadPackage(IEnumerable<string> assemblyPaths)
		{
			var result = new PackageDefinition();
			foreach (var assemblyPath in assemblyPaths)
			{
				var assembly = AssemblyDefinition.ReadAssembly(assemblyPath);
				var types = assembly.Modules.SelectMany(m => m.Types).Where(t => t.IsPublic);
				result.TypeDefinitions.AddRange(types);

				foreach (var type in types)
					AddExposedTypes(type, result);
			}

			return GetDefs(result).Keys;
		}

		public PackageDefinition AddExposedTypes(TypeDefinition type, PackageDefinition package)
		{
			var exposedTypes = new List<TypeDefinition>();

			var methods = type.Methods.Where(m => m.IsPublic);
			exposedTypes.AddRange(methods
				.SelectMany(m => m.Parameters.Select(p => p.ParameterType.Resolve()).Union(new[] { m.ReturnType.Resolve() }))
				.Where(m => m != null && package.TypeDefinitions.Select(t => t.FullName).Contains(m.FullName) == false));
			package.MethodDefinitions.AddRange(methods);

			var fields = type.Fields.Where(m => m.IsPublic);
			exposedTypes.AddRange(fields
				.Select(m => m.FieldType.Resolve())
				.Where(m => m != null && package.TypeDefinitions.Select(t => t.FullName).Contains(m.FullName) == false));
			package.FieldDefinitions.AddRange(fields);

			var nested = type.NestedTypes.Where(m => m.IsNestedPublic);
			exposedTypes.AddRange(nested
				.Select(m => m.Resolve())
				.Where(m => m != null && package.TypeDefinitions.Select(t => t.FullName).Contains(m.FullName) == false));
			package.TypeDefinitions.AddRange(nested);

			//TODO: make the inclusion or exclusion of system a feature.
			package.TypeDefinitions.AddRange(exposedTypes.Distinct());

			foreach (var t in exposedTypes)
				AddExposedTypes(t, package);

			return package;
		}

		private Dictionary<string, IMemberDefinition> GetDefs(PackageDefinition package)
		{
			return package
				.TypeDefinitions.Cast<IMemberDefinition>()
				.Union(package.MethodDefinitions.Cast<IMemberDefinition>())
				.Union(package.FieldDefinitions.Cast<IMemberDefinition>())
				.ToDictionary(t => t.FullName, t => t);
		}
	}
}
