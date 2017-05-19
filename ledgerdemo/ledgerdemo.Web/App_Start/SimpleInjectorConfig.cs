using ledgerdemo.Services;
using ledgerdemo.Services.Account;
using ledgerdemo.Services.User;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace ledgerdemo.Web.App_Start {
    public class SimpleInjectorConfig {
        public static void RegisterDependencies() {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            var serviceFactory = ServiceFactory.Instance();
            container.Register<IUserService>(() => serviceFactory.GetService<IUserService>());
            container.Register<IAccountService>(() => serviceFactory.GetService<IAccountService>());

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcIntegratedFilterProvider();
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}