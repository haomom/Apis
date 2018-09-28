using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Mizuho.London.Common.Interfaces.Web;

namespace Mizuho.London.FinanceLedgerPosting.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents();
            AutoMapperConfig.Configure();

            DependencyResolver.Current.GetServices<ITaskApplicationStart>().ToList().ForEach(x => x.Execute());
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.StatusCode = 200;
                var httpApplication = sender as HttpApplication;
                httpApplication?.CompleteRequest();
            }
        }
    }
}
