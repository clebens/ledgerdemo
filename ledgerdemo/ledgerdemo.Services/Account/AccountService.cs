using ledgerdemo.Services.Account.Persistence;
using ledgerdemo.Services.DBTableTypes.ConstantTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ledgerdemo.Services.Account
{
    public interface IAccountService {
        void CommitTransaction(TransactionType type, decimal amount);
        decimal GetBalance(int accountid);
    }

    public class AccountService : IAccountService {
        private IAccountRepository AccountRepository;

        public AccountService(IAccountRepository accountRepository) {
            this.AccountRepository = accountRepository;
        }

        public void CommitTransaction(TransactionType type, decimal amount) {
            throw new NotImplementedException();
        }

        public decimal GetBalance(int accountid) {
            throw new NotImplementedException();
        }
    }
}
