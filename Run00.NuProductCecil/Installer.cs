using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Run00.NuProduct;

namespace Run00.NuProductCecil
{
	public class Installer : IWindsorInstaller
	{
		void IWindsorInstaller.Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<IPackageReader>().ImplementedBy<PackageReader>(),
				Component.For<ISemanticVersioning>().ImplementedBy<SemanticVersioning>(),
				Component.For<INuGetFactory>().ImplementedBy<NuGetFactory>(),
				Component.For<IRunner>().ImplementedBy<Runner>()
			);
		}

	}
}
