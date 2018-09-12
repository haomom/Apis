using Microsoft.Practices.Unity;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using System.Web.Http;
using Unity.WebApi;

namespace Mizuho.London.FinanceLedgerPosting.WebApi
{
    public static class UnityConfig
    {
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