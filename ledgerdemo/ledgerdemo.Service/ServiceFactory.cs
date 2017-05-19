using ledgerdemo.Services.Account;
using ledgerdemo.Services.Account.Persistence;
using ledgerdemo.Services.User;
using ledgerdemo.Services.User.Persistence;
using SimpleInjector;
using System;

namespace ledgerdemo.Services
{
    /* NOTE - This is NOT the ideal design, but works for a prototype. Ideally, each service would have a factory class
     * that builds out its own dependency tree. This works well for prototyping on this level, however (particularly until
     * you know more about service lifecycle management).
     */
     
    public class ServiceFactory {
        private Container container;
        private static ServiceFactory _instance;

        private ServiceFactory() {
            container = new Container();
            container.Register<IDB, MockDB>(lifestyle: Lifestyle.Singleton);

            container.Register<IAccountRepository, AccountRepository>();
            container.Register<IAccountService, AccountService>();

            container.Register<IUserRepository, UserRepository>();
            container.Register<IUserService, UserService>();

            container.Verify();
        }

        public T GetService<T>() where T : class {
            return container.GetInstance<T>();
        }

        public static ServiceFactory Instance() { 
            if (_instance != null) return _instance;

            _instance = new ServiceFactory();
            return _instance;
        }
    }
}
