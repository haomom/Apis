using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using System.Web.Http;
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