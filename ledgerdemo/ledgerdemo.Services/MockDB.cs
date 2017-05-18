using ledgerdemo.Services.DBTableTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ledgerdemo.Services
{
    public interface IDB {
        ICollection<accounts> accounts { get; }
        ICollection<users> users { get; }
        ICollection<transactions> transactions { get; }

    }

    public class MockDB : IDB {
        public ICollection<accounts> accounts { get; set; }
        public ICollection<users> users { get; set; }
        public ICollection<transactions> transactions { get; set; }

        public MockDB() {
            this.accounts = new List<accounts>();
            this.users = new List<users>();
            this.transactions = new List<transactions>();
        }
    }
}
