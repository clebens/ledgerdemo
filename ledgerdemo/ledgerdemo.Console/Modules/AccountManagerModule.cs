using ledgerdemo.ConsoleApp.Helpers;
using ledgerdemo.Service;
using ledgerdemo.Services.Account;
using ledgerdemo.Services.DBTableTypes.ConstantTypes;
using ledgerdemo.Services.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ledgerdemo.ConsoleApp.Modules {
    public class AccountManagerModule {
        private IAccountService AccountService;

        public AccountManagerModule(IAccountService accountService) {
            this.AccountService = accountService;
        }

        public void Deposit(int accountid) {
            var amount = decimal.Parse(PromptGenerator.Input("Amount"));
            AccountService.Deposit(accountid, amount);
        }

        public void Withdraw(int accountid) {
            var amount = decimal.Parse(PromptGenerator.Input("Amount"));
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
                input = PromptGenerator.Menu(new List<string>() {
                        "Deposit",
                        "Withdraw",
                        "List Transactions",
                        "Print Balance"
                    });
                try {
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
                } catch (DisplayedException ex) {
                    Console.WriteLine(ex.Message);
                } catch (Exception ex) {
                    Console.WriteLine("System error.");
                }
            } while (input != "0");
        }
    }
}
