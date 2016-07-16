using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using RestApi.Models;

namespace RestApi.CastleWindsorConfig
{
    public class ContextsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IPatientContext>().ImplementedBy<PatientContext>().LifestylePerWebRequest());
        }
    }
}