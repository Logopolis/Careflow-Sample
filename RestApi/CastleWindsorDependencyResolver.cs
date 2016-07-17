﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using System.Web.Http;
using Castle.MicroKernel.Lifestyle;

namespace RestApi
{
    // Adapted from http://www.codeproject.com/Articles/710662/Simplest-Possible-ASP-NET-Web-API-Project-that-Imp
    public class CastleWindsorDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer _container;

        public CastleWindsorDependencyResolver(IWindsorContainer container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(_container);
        }

        public object GetService(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType) ? _container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (!_container.Kernel.HasComponent(serviceType))
            {
                return new object[0];
            }

            return _container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }

    public class WindsorDependencyScope : IDependencyScope
    {
        private readonly IWindsorContainer _container;
        private readonly IDisposable _scope;

        public WindsorDependencyScope(IWindsorContainer container)
        {
            _container = container;
            _scope = container.BeginScope();
        }

        public object GetService(Type serviceType)
        {
            if (_container.Kernel.HasComponent(serviceType))
            {
                return _container.Resolve(serviceType);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }

    public class ApiControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container,
        Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
             .BasedOn<ApiController>()
             .LifestyleTransient());
        }
    }
}