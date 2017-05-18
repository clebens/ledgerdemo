using ledgerdemo.ConsoleApp.Helpers;
using ledgerdemo.Service;
using ledgerdemo.Services.Account;
using ledgerdemo.Services.User;
using ledgerdemo.Services.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ledgerdemo.ConsoleApp.Modules {
    public class MainModule {
        private IAccountService AccountService;
        private IUserService UserService;
        private AccountManagerModule AccountModule;
        public MainModule(IAccountService accountService, IUserService userService) {
            this.AccountService = accountService;
            this.UserService = userService;
            this.AccountModule = new AccountManagerModule(accountService);
        }
        
        public void CreateAccount() {
            var email = PromptGenerator.Input("Email");
            var password = PromptGenerator.Input("Password", hide: true);

            var user = UserService.CreateUser(new Services.User.DTOs.CreateUserModel {
                email = email,
                password = password
            });

            AccountService.CreateAccountForUser(user.userid);
            return;
        }

        public UserViewModel Login() {
            var email = PromptGenerator.Input("Email");
            var password = PromptGenerator.Input("Password", hide: true);

            if (UserService.AuthenticateUser(email, password))
                return UserService.GetUserByEmail(email);

            return null;
        }

        public void MainMenu() {
            string input;
            do {
                input = PromptGenerator.Menu(new List<string>() {
                        "Create Account",
                        "Manage Account"
                    });
                try {
                    switch (input) {
                        case "1": CreateAccount(); break;
                        case "2":
                            var user = Login();
                            if (user != null)
                                AccountModule.AccountMenu(user);
                            else
                                Console.WriteLine("User login failed.");
                            break;
                        default:
                            break;
                    }
                } catch (DisplayedException ex) {
                    Console.WriteLine(ex.Message);
                } catch (Exception ex) {
                    Console.WriteLine("System error.");
                }
            } while (input != "0");
        }
    }
}
