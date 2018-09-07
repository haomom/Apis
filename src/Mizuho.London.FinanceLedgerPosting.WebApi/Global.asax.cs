using System.Linq;
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

            DependencyResolver.Current.GetServices<ITaskApplicationStart>().ToList().ForEach(x => x.Execute());
        }
    }
}
