using System;
using System.Data.Entity;
using System.Web;
using DataAccessLayer;
using DataAccessLayer.Entities;
using ExpensesDomain.Repositories;
using ExpensesDomain.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Practices.Unity;

namespace WspolnaKasa.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<DbContext, ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new PerRequestLifetimeManager());
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IGroupRepository, GroupRepository>();
            container.RegisterType<IExpensesRepository, ExpensesRepository>();
            container.RegisterType<ITransferRepository, TransferRepository>();
            container.RegisterType<IApplicationUserRepository, ApplicationUserRepository>();

            container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<ITransactionService, TransactionService>();
        }        
    }
}