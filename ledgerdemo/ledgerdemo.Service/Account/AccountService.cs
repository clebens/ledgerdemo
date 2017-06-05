using ledgerdemo.Service;
using ledgerdemo.Service.Account.DTOs;
using ledgerdemo.Services.Account.Persistence;
using ledgerdemo.Services.DBTableTypes;
using ledgerdemo.Services.DBTableTypes.ConstantTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ledgerdemo.Services.Account
{
    public interface IAccountService {
        void Deposit(int accountid, decimal amount);
        void Withdraw(int accountid, decimal amount);
        decimal GetBalance(int accountid);
        List<TransactionView> GetTransactionLogForAccount(int accountid);
        void CreateAccountForUser(int userid);
        AccountViewModel GetAccountForUser(int userid);
        AccountViewModel GetAccount(int accountid);
    }

    public class AccountService : IAccountService {
        private IAccountRepository AccountRepository;

        public AccountService(IAccountRepository accountRepository) {
            this.AccountRepository = accountRepository;
        }

        public void CreateAccountForUser(int userid) {
            if (AccountRepository.GetAccountsForUser(userid).Count > 0)
                throw new DisplayedException("Currently supporting only one account per user.");


            AccountRepository.CreateAccount(new accounts {
                userid = userid,
                balance = 0.0M
            });
        }

        public void Deposit(int accountid, decimal amount) {
            if (amount < 0.01M) throw new DisplayedException("Illegal Deposit Amount.");
            if (getPostDecimalDigitCount(amount) > 2) throw new DisplayedException("Must Deposit in .01 Increments.");
            var account = AccountRepository.GetAccount(accountid);
            account.balance += amount;
            transactions t = new transactions {
                accountid = accountid,
                adjustment = amount,
                type = TransactionType.DEPOSIT,
                datecreated = DateTimeOffset.UtcNow
            };
            using (var tscope = new TransactionScope()) {
                AccountRepository.UpdateAccount(account);
                AccountRepository.LogTransaction(t);
            }
        }

        public AccountViewModel GetAccount(int accountid) {
            var dbAccount = AccountRepository.GetAccount(accountid);
            return new AccountViewModel {
                accountid = dbAccount.accountid,
                balance = dbAccount.balance,
                userid = dbAccount.userid
            };
        }

        public AccountViewModel GetAccountForUser(int userid) {
            var dbAccount = AccountRepository.GetAccountsForUser(userid).FirstOrDefault();
            return new AccountViewModel {
                accountid = dbAccount.accountid,
                balance = dbAccount.balance,
                userid = dbAccount.userid
            };
        }

        public decimal GetBalance(int accountid) {
            var account = AccountRepository.GetAccount(accountid);
            return account.balance;
        }

        public List<TransactionView> GetTransactionLogForAccount(int accountid) {
            var dbtrans = AccountRepository.GetTransactionsForAccount(accountid);
            return dbtrans.Select(x => {
                return new TransactionView {
                    accountid = x.accountid,
                    adjustment = x.adjustment,
                    transactionid = x.transactionid,
                    type = x.type,
                    datecreated = x.datecreated
                };
            }).ToList();
        }

        private int getPostDecimalDigitCount(decimal v) {
            return BitConverter.GetBytes(decimal.GetBits((decimal)(double)v)[3])[2];
        }

        public void Withdraw(int accountid, decimal amount) {
            if (amount < 0.01M) throw new DisplayedException("Illegal Withdrawal Amount.");
            if (getPostDecimalDigitCount(amount) > 2) throw new DisplayedException("Must Withdraw in .01 Increments.");
            var account = AccountRepository.GetAccount(accountid);
            if (amount > account.balance) throw new DisplayedException("Insufficient Funds for Withdrawal.");
            account.balance -= amount;
            transactions t = new transactions {
                accountid = accountid,
                adjustment = (-1.0M) * amount,
                type = TransactionType.WITHDRAWAL, 
                datecreated = DateTimeOffset.UtcNow
            };
            using (var tscope = new TransactionScope()) {
                AccountRepository.UpdateAccount(account);
                AccountRepository.LogTransaction(t);
            }
        }
    }
}
