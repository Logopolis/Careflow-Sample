﻿using Castle.Windsor;
using RestApi.CastleWindsorConfig;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace RestApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IWindsorContainer container)
        {
            config.Routes.MapHttpRoute(
                name: "Patients and episodes",
                routeTemplate: "patients/{patientId}/episodes",
                defaults: new {controller = "Patients", action = "Get", patientId = RouteParameter.Optional});

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(container));
        }
    }
}
