using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace Mizuho.London.FinanceLedgerPosting.WebApi.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
        {
            Thread.CurrentPrincipal = User;
        }
    }
}
