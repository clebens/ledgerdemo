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
        private class Application {
            private IAccountService AccountService;
            private IUserService UserService;

            public Application(IAccountService accountService, IUserService userService) {
                this.AccountService = accountService;
                this.UserService = userService;
            }

            private string ReadLineHidden() {
                string input = "";
                while (true) {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter) break;
                    else input += key.KeyChar;
                } 
                return input;
            }

            private void printMenu(List<string> items) {
                var str = "";
                for (var i = 0; i < items.Count; i++) {
                    str += $"{i+1}. {items[i]}\n";
                }
                str += "->";
                Console.WriteLine(str);
            }

            public void CreateAccount() {
                Console.WriteLine("\n\nEmail: ");
                var email = Console.ReadLine();

                Console.WriteLine("\n\nPassword: ");
                var password = ReadLineHidden();

                var user = UserService.CreateUser(new Services.User.DTOs.CreateUserModel {
                    email = email,
                    password = password
                });

                AccountService.CreateAccountForUser(user.userid);
                return;
            }

            public UserViewModel Login() {
                Console.WriteLine("\n\nEmail: ");
                var email = Console.ReadLine();

                Console.WriteLine("\n\nPassword: ");
                var password = ReadLineHidden();

                if  (UserService.AuthenticateUser(email, password))
                    return UserService.GetUserByEmail(email);

                return null;
            }

            public void Deposit(int accountid) {
                Console.WriteLine("Amount: ");
                var amount = decimal.Parse(Console.ReadLine());
                AccountService.Deposit(accountid, amount);
            }

            public void Withdraw(int accountid) {
                Console.WriteLine("Amount: ");
                var amount = decimal.Parse(Console.ReadLine());
                AccountService.Withdraw(accountid, amount);
            }

            public void PrintBalance(int accountid) {
                Console.WriteLine($"Balance: {AccountService.GetBalance(accountid)}");
            }

            public void PrintTransactions(int accountid) {
                var transactions = AccountService.GetTransactionLogForAccount(accountid);
                transactions.ForEach(x => {
                    Console.WriteLine(
                        $"id: {x.transactionid}, " +
                        $"type: {Enum.GetName(typeof(TransactionType), x.type)}, " +
                        $"adjustment: {x.adjustment}, " +
                        $"time: {x.datecreated}\n");
                });
            }
            public void AccountMenu(UserViewModel user) {
                string input;

                var account = AccountService.GetAccountForUser(user.userid);

                do {
                    printMenu(new List<string>() {
                        "Deposit",
                        "Withdraw",
                        "List Transactions",
                        "Print Balance"
                    });
                    input = Console.ReadLine();
                    switch (input) {
                        case "1":
                            Deposit(account.accountid);
                            PrintBalance(account.accountid);
                            break;
                        case "2":
                            Withdraw(account.accountid);
                            PrintBalance(account.accountid);
                            break;
                        case "3":
                            PrintTransactions(account.accountid);
                            break;
                        case "4":
                            PrintBalance(account.accountid);
                            break;
                        default:
                            break;
                    }
                } while (input != "0");
            }

            public void MainMenu() {
                string input;
                do {
                    printMenu(new List<string>() {
                        "Create Account",
                        "Manage Account"
                    });
                    input = Console.ReadLine();
                    switch (input) {
                        case "1": CreateAccount(); break;
                        case "2":
                            var user = Login();
                            if (user != null)
                                AccountMenu(user);
                            else
                                Console.WriteLine("User login failed.");
                            break;
                        default:
                            break;
                    }
                } while (input != "0");
            }
        }

        static void Main(string[] args) {
            var serviceFactory = ServiceFactory.Instance();
            new Application(
                serviceFactory.GetService<IAccountService>(), 
                serviceFactory.GetService<IUserService>()
            ).MainMenu();
        }
    }
}
