using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using RTDemoProject.Entities;
using RTDemoProject.AutoMaper;
using RTDemoProject.Entities.Interfaces;
using RTDemoProject.Entities.POCOs;
using AutoMapper;
using RTDemoProject.Service;
using System.Web.Http;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using RTDemoProject.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;
using RTDemoProject.Repository;
using RTDemoProject.Repository.Interfaces;

namespace RTDemoProject.App_Start
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
            var mapper = CommonMapper.InitializeAutoMapper().CreateMapper();
        
            container
               .RegisterInstance<IMapper>(mapper)
               .RegisterInstance<HttpConfiguration>(GlobalConfiguration.Configuration)
               .RegisterType<ApplicationUserManager>()

                .RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>()
                .RegisterType<UserManager<ApplicationUser>>()
                .RegisterType<ApplicationUserManager>()
                .RegisterType<AccountController>(new InjectionConstructor())

                .RegisterType<ITextEncoder, Base64UrlTextEncoder>()
                .RegisterType<IDataSerializer<AuthenticationTicket>, TicketSerializer>()
                .RegisterInstance(new DpapiDataProtectionProvider().Create("ASP.NET Identity"))
                .RegisterType<ISecureDataFormat<AuthenticationTicket>, SecureDataFormat<AuthenticationTicket>>()

               .RegisterType(typeof(ISecureDataFormat<>), typeof(SecureDataFormat<>))
               .RegisterType<IDataContextAsync, EFContext>(new PerRequestLifetimeManager())
               .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager())
               .RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager())
               .RegisterType<IRepositoryAsync<Employee>, Repository<Employee>>()
               .RegisterType<IRepositoryAsync<Contact>, Repository<Contact>>()
               .RegisterType<IRepositoryAsync<Site>, Repository<Site>>()
               .RegisterType<IRepositoryAsync<Order>, Repository<Order>>()
               .RegisterType<IRepositoryAsync<SalesOrderDetail>, Repository<SalesOrderDetail>>()
               .RegisterType<IRepositoryAsync<ApplicationUser>, Repository<ApplicationUser>>()
               .RegisterType<IEmployeeService, EmployeeService>()
               ;



        }
    }
}
