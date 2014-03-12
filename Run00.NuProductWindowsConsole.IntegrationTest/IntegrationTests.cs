using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using Roslyn.Services.CSharp.Classification;
using Run00.MsTest;
using Run00.NuProductVersioning;
using System.IO;

namespace Run00.NuProductWindowsConsole.IntegrationTest
{
	[DeploymentItem(@"..\..\Artifacts")]
	[TestClass, CategorizeByConventionClass(typeof(IntegrationTest))]
	public class IntegrationTest
	{
		[TestInitialize]
		public void Initialize()
		{
			_installPath = Path.Combine(Path.GetTempPath(), "NumericTests");
		}

		[TestMethod, CategorizeByConvention]
		public void WhenOnlyCodeBlockIsChanged_ShouldBeRefactor()
		{
			//Arrange;
			var args = GetArgs("RefactoringMethod");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Refactor, result.Justification.ChangeType);
			Assert.AreEqual("1.0.1.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenOnlyCommentsAreChanged_ShouldBeCosmetic()
		{
			//Arrange
			var args = GetArgs("ChangingComments");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Cosmetic, result.Justification.ChangeType);
			Assert.AreEqual("1.0.1.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenClassIsDeleted_ShouldBeBreaking()
		{
			//Arrange
			var args = GetArgs("DeletingClass");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenClassIsAdded_ShouldBeEnhancement()
		{
			//Arrange
			var args = GetArgs("AddingClass");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Enhancement, result.Justification.ChangeType);
			Assert.AreEqual("1.1.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenMethodIsAdded_ShouldBeEnhancement()
		{
			//Arrange
			var args = GetArgs("AddingMethod");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Enhancement, result.Justification.ChangeType);
			Assert.AreEqual("1.1.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenMethodIsModified_ShouldBeBreaking()
		{
			//Arrange
			var args = GetArgs("ChangingMethodSignature");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenNamespaceIsChanged_ShouldBeBreaking()
		{
			//Arrange
			var args = GetArgs("ChangingNamespace");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenGenericIsChanged_ShouldBeBreaking()
		{
			//Arrange
			var args = GetArgs("ChangingGenericType");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenPrivateMethodIsAdded_ShouldBeRefactor()
		{
			//Arrange
			var args = GetArgs("AddingPrivateMethod");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Refactor, result.Justification.ChangeType);
			Assert.AreEqual("1.0.1.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenPropertyIsAdded_ShouldBeEnhancement()
		{
			//Arrange
			var args = GetArgs("AddingProperty");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Refactor, result.Justification.ChangeType);
			Assert.AreEqual("1.1.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenEnumIsAdded_ShouldBeEnhancement()
		{
			//Arrange
			var args = GetArgs("AddingEnum");

			//Act
			var result = Program.Run(args);

			//Assert
			//Assert.AreEqual(ContractChangeType.Refactor, result.Justification.ChangeType);
			Assert.AreEqual("1.1.0.0", result.New.ToString());
		}

		[TestCleanup]
		public void Cleanup()
		{
			//This is necessary because the Package Manager does not overwrite the installation folder
			//and the version of the test packages never changes.
			if (Directory.Exists(_installPath))
				Directory.Delete(_installPath, true);
		}

		private string[] GetArgs(string projectId)
		{
			//var w = Workspace.LoadSolution(@"C:\SourceCode\Run00\Versioning.Roslyn\Run00.VersioningRoslyn.IntegrationTest\Artifacts\Adding\Test.Sample.sln");
			//var s = w.CurrentSolution;
			//foreach (var p in s.Projects)
			//	p.ToString();

			//var packageDir = Directory.GetCurrentDirectory();
			//var packagePath = Path.Combine(packageDir, "Test.Sample." + projectId + ".0.0.0.nupkg");
			//return new[] { packagePath, packageDir, _installPath, "Test.Sample.ControlGroup" };

			return new[] { @"..\..\..\Run00.NuProduct.roslyn.sln", "Test.Sample." + projectId };
		}

		private string _installPath;

	}
}
