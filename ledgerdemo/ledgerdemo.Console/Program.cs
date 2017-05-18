using ledgerdemo.ConsoleApp.Modules;
using ledgerdemo.Services;
using ledgerdemo.Services.Account;
using ledgerdemo.Services.DBTableTypes.ConstantTypes;
using ledgerdemo.Services.User;
using ledgerdemo.Services.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ledgerdemo.ConsoleApp {
    class Program {
        

        static void Main(string[] args) {
            var serviceFactory = ServiceFactory.Instance();
            new MainModule(
                serviceFactory.GetService<IAccountService>(), 
                serviceFactory.GetService<IUserService>()
            ).MainMenu();
        }
    }
}
