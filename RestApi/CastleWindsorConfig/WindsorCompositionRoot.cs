using Castle.Windsor;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace RestApi.CastleWindsorConfig
{
    // Adapted from http://www.codeproject.com/Articles/710662/Simplest-Possible-ASP-NET-Web-API-Project-that-Imp
    public class WindsorCompositionRoot : IHttpControllerActivator
    {
        private readonly IWindsorContainer _container;

        public WindsorCompositionRoot(IWindsorContainer container)
        {
            _container = container;
        }

        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var controller =
                (IHttpController)_container.Resolve(controllerType);

            request.RegisterForDispose(
                new _Release(
                    () => _container.Release(controller)));

            return controller;
        }

        private sealed class _Release : IDisposable
        {
            private readonly Action _release;

            public _Release(Action release)
            {
                _release = release;
            }

            public void Dispose()
            {
                _release();
            }
        }
    }
}