using ledgerdemo.Service;
using ledgerdemo.Services.DBTableTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ledgerdemo.Services.Account.Persistence
{
    public interface IAccountRepository {
        transactions LogTransaction(transactions transaction);
        List<accounts> GetAccountsForUser(int userid);
        accounts CreateAccount(accounts account);
        accounts UpdateAccount(accounts account);
        accounts GetAccount(int accountid);
        List<transactions> GetTransactionsForAccount(int accountid);
    }
    public class AccountRepository : IAccountRepository {
        private IDB DB;

        public AccountRepository(IDB db) {
            this.DB = db;
        }

        public accounts CreateAccount(accounts account) {
            if (account.accountid != 0) throw new Exception("Accountid must be empty.");
            account.accountid = (DB.accounts.Count > 0) ? DB.accounts.Max(x => x.accountid) + 1 : 1;
            DB.accounts.Add(account);
            return account;
        }

        public accounts GetAccount(int accountid) {
            return DB.accounts.FirstOrDefault(x => x.accountid == accountid);
        }

        public List<accounts> GetAccountsForUser(int userid) {
            return DB.accounts.Where(x => x.userid == userid).ToList();
        }

        public List<transactions> GetTransactionsForAccount(int accountid) {
            return DB.transactions.Where(x => x.accountid == accountid).ToList();
        }

        public transactions LogTransaction(transactions transaction) {
            if (transaction.transactionid != 0) throw new Exception("Accountid must be empty.");
            transaction.transactionid = (DB.transactions.Count > 0) ? DB.transactions.Max(x => x.transactionid) + 1 : 1;
            this.DB.transactions.Add(transaction);
            return transaction;
        }

        public accounts UpdateAccount(accounts account) {
            var dbaccount = DB.accounts.First(x => x.accountid == account.accountid);
            dbaccount = account;
            return account;
        }
    }
}
