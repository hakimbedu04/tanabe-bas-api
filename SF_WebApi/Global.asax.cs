using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SF_Domain.Mappers;
using SF_WebApi.MapperViewModel;

namespace SF_WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapper.Mapper.Initialize(x =>
            {
                x.AddProfile<ViewMappingProfiles>();

                x.AddProfile<hrd_MapperProfiles>();
                x.AddProfile<bas_MapperProfiles>();
                x.AddProfile<prod_MapperProfiles>();
            });
        }
    }
}
