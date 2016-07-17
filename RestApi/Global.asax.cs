using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Web.Http;
using System.Web.Mvc;

namespace RestApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;

        protected void Application_Start()
        {
            ConfigureWindsor(GlobalConfiguration.Configuration);

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration, _container);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        public static IWindsorContainer ConfigureWindsor(HttpConfiguration configuration)
        {
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));
            var dependencyResolver = new CastleWindsorDependencyResolver(_container);
            configuration.DependencyResolver = dependencyResolver;
            return _container;
        }

        protected void Application_End()
        {
            _container.Dispose();
            base.Dispose();
        }
    }
}