using System;
using System.Collections.Generic;
using System.Text;

namespace ledgerdemo.Services.Account.Persistence
{
    public interface IAccountRepository { }
    public class AccountRepository : IAccountRepository {
        private IDB DB;

        public AccountRepository(IDB db) {
            this.DB = db;
        }
    }
}
