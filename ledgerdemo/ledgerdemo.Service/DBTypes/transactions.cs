using ledgerdemo.Services.DBTableTypes.ConstantTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ledgerdemo.Services.DBTableTypes
{
    public class transactions {
        public int transactionid { get; set; }
        public int accountid { get; set; }
        public decimal adjustment { get; set; }
        public DateTimeOffset datecreated { get; set; }
        public TransactionType type { get; set; }
    }
}
