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
				Component.For<INuGet>().ImplementedBy<NuGet>(),
				Component.For<IPackageReader>().ImplementedBy<PackageReader>(),
				Component.For<NuGetConfiguration>().Instance(new NuGetConfiguration()).LifestyleSingleton()
			);
		}

	}
}
