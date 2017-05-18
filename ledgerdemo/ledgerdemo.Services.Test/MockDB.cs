using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ledgerdemo.Services.DBTableTypes;

namespace ledgerdemo.Services.Test {
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
