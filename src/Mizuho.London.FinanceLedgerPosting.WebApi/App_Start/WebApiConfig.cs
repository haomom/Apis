﻿using Mizuho.London.FinanceLedgerPosting.WebApi.Filter;
using System.Web.Http;
using System.Web.Http.Cors;
using Mizuho.London.Common.RoleProviders;

namespace Mizuho.London.FinanceLedgerPosting.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web Api enable CORS
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Ignore Items in circular reference
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            config.Filters.Add(new MizuhoAuthorizeWebApi());
            config.Filters.Add(new AppExceptionFilterAttribute());
        }
    }
}
