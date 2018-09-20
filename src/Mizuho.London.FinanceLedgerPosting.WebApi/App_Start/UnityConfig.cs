using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using System.Web.Http;
using Mizuho.London.Common.Interfaces.Logging;
using Mizuho.London.Common.Logging;
using Mizuho.London.FinanceLedgerPosting.Services;
using Unity.WebApi;
using Unity;
using Unity.Lifetime;

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

            // register mizuho log service
            container.RegisterType<IMizLog, MizLog>();
            container.RegisterType<IMizLogConfigHandler, MizLogConfigHandler>();

            container.RegisterType<IUnitOfWork, FinanceLedgerPostingDbContext>(new HierarchicalLifetimeManager());

            #region Model
            container.RegisterType<ISuspenseAccount, SuspenseAccount>();
            container.RegisterType<IUserCredential, UserCredential>();
            #endregion

            #region Services
            container.RegisterType<IUserCredentialService, UserCredentialService>();
            container.RegisterType<IUserCredentialsSource, UserCredentialsSource>();
            #endregion

            #region Repository

            container.RegisterType<ISuspenseAccountRepository, SuspenseAccountRepository>();

            #endregion

            return container;
        }
    }
}