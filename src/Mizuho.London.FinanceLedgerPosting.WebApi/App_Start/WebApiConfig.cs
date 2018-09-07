using System.Web.Http;
using Mizuho.London.FinanceLedgerPosting.WebApi.Filter;

namespace Mizuho.London.FinanceLedgerPosting.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web Api enable CORS
            //config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();


            #region Suspense Account

            config.Routes.MapHttpRoute(
                name: "CreateSuspenseAccount",
                routeTemplate: "api/suspenseaccount/createsuspenseaccount",
                defaults: new { controller = "SuspenseAccount", action = "CreateSuspenseAccount" }
            );

            #endregion

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Ignore Items in circular reference
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //config.Filters.Add(new AppExceptionFilterAttribute());
        }
    }
}
