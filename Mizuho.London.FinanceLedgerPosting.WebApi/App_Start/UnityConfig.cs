using System.Web.Http;
using Mizuho.London.FinanceLedgerPosting.Common.Data;
using Mizuho.London.FinanceLedgerPosting.Interfaces.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Interfaces.Model;
using Mizuho.London.FinanceLedgerPosting.Interfaces.Repository;
using Mizuho.London.FinanceLedgerPosting.Repository;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace Mizuho.London.FinanceLedgerPosting.WebApi
{
    /// <summary>
    /// This class is for mapping all components for IOC
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Register all components for IOC
        /// </summary>
        public static void RegisterComponents()
        {
			var container = BuildUnityContainer();
           
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            
            container.RegisterType<IUnitOfWork, FinanceLedgerPostingDbContext>(new HierarchicalLifetimeManager());

            #region Model
            container.RegisterType<ISuspenseAccount, SuspenseAccount>();
            #endregion

            #region Repository

            container.RegisterType<ISuspenseAccountRepository, SuspenseAccountRepository>();

            #endregion

            return container;
        }
    }
}