using System.Web.Http;
using Ninject.Web.Common.WebHost;
using SF_BusinessLogics.ErrLogs;
using SF_BusinessLogics.LoginBLL;
using SF_BusinessLogics.Offline;
using SF_BusinessLogics.SP;
using SF_BusinessLogics.Visit;
using SF_Repositories.Common;
using SF_Repositories.LoginRepo;
using SF_Repositories.SPRepo;
using SF_Repositories.VisitRepo;
using WebApiContrib.IoC.Ninject;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SF_WebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SF_WebApi.App_Start.NinjectWebCommon), "Stop")]

namespace SF_WebApi.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using SF_BusinessLogics.Dashboard;
    using SF_BusinessLogics.User;
    using SF_BusinessLogics.GeneralExpense;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                //Note: Add the line below:
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            #region Login

            kernel.Bind<ILoginBLL>().To<LoginBLL>().InRequestScope();
            kernel.Bind<ILoginRepo>().To<LoginRepo>().InRequestScope();
            
            #endregion

            #region visit
            //kernel.Bind<IVisitPlanBLL>().To<VisitPlanBLL>().InRequestScope();
            //kernel.Bind<IVisitRealizationBLL>().To<VisitRealizationBLL>().InRequestScope();
            kernel.Bind<IVisitRepo>().To<VisitRepo>().InRequestScope();
            kernel.Bind<ISqlSPRepository>().To<SqlSPRepository>().InRequestScope();
            kernel.Bind<IVisitBLL>().To<VisitBLL>().InRequestScope();
            kernel.Bind<IVTLogger>().To<VTLogger>().InRequestScope();
            kernel.Bind<IJsonFilesGenerator>().To<JsonFilesGenerator>().InRequestScope();
            
            #endregion

            #region SP
            kernel.Bind<ISPPlanBLL>().To<SPPlanBLL>().InRequestScope();
            kernel.Bind<ISPActualBLL>().To<SPActualBLL>().InRequestScope();
            kernel.Bind<ISPReportBLL>().To<SPReportBLL>().InRequestScope();
            kernel.Bind<ISPRealizationBLL>().To<SPRealizationBLL>().InRequestScope();
            kernel.Bind<ISPRepo>().To<SPRepo>().InRequestScope();
            #endregion

            kernel.Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>)).InRequestScope();
            kernel.Bind(typeof(IBasGenericRepositories<>)).To(typeof(BasGenericRepositories<>)).InRequestScope();

            #region dashboard
            kernel.Bind<IDashboardBLL>().To<DashboardBLL>().InRequestScope();
            #endregion

            #region user
            kernel.Bind<IUserPlanBLL>().To<UserPlanBLL>().InRequestScope();
            kernel.Bind<IUserRealizationBLL>().To<UserRealizationBLL>().InRequestScope();
            kernel.Bind<IUserActualBLL>().To<UserActualBLL>().InRequestScope();
            kernel.Bind<IUserHistoryBLL>().To<UserHisotryBLL>().InRequestScope();
            #endregion

            #region generalexpense
            kernel.Bind<IGeneralExpense>().To<GeneralExpense>().InRequestScope();
            #endregion
        }
    }
}