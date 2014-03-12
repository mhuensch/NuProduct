using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Run00.NuProduct;
using System;

namespace Run00.NuProductVersioning
{
	public class Installer : IWindsorInstaller
	{
		void IWindsorInstaller.Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<ISemanticVersioning>().ImplementedBy<SemanticVersioning>());
		}
	}
}
