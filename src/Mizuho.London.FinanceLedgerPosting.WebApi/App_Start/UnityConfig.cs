using Mizuho.London.Common.Logging;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using Mizuho.London.FinanceLedgerPosting.Services;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
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

            // register mizuho log service
            container.RegisterType<IMizLog, MizLog>();
            container.RegisterType<IMizLogConfigHandler, MizLogConfigHandler>();


            container.RegisterType<IUnitOfWork, FinanceLedgerPostingDbContext>(new HierarchicalLifetimeManager());
            
            #region Model
            container.RegisterType<ISuspenseAccount, SuspenseAccount>();
            container.RegisterType<IUserCredential, UserCredential>();
            container.RegisterType<IBranch, Branch>();
            #endregion

            #region Services
            container.RegisterType<IUserCredentialService, UserCredentialService>();
            container.RegisterType<IBranchService, BranchService>();
            #endregion

            #region Repository

            container.RegisterType<ISuspenseAccountRepository, SuspenseAccountRepository>();
            container.RegisterType<IUserCredentialRepository, UserCredentialRepository>();
            container.RegisterType<IBranchRepository, BranchRepository>();

            #endregion

            return container;
        }
    }
}