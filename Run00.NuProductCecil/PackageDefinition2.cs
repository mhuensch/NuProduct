using Mono.Cecil;
using System.Collections.Generic;

namespace Run00.NuProductCecil
{
	public class PackageDefinition2
	{
		public ICollection<TypeDefinition> TypeDefinitions { get; set; }
		public ICollection<MethodDefinition> MethodDefinitions { get; set; }
		public ICollection<FieldDefinition> FieldDefinitions { get; set; }

		public PackageDefinition2()
		{
			TypeDefinitions = new List<TypeDefinition>();
			MethodDefinitions = new List<MethodDefinition>();
			FieldDefinitions = new List<FieldDefinition>();
		}
	}
}
