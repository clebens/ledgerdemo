using System;
using System.Collections.Generic;
using System.Text;

namespace ledgerdemo.Services.DBTableTypes.ConstantTypes
{
    public enum TransactionType {
        DEPOSIT = 1,
        WITHDRAWAL,
        // NOTE - I wouldn't include the types below based purely on the spec, but illustrating
        // that I used a enumerated type for future expansion (rather than just 
        // assuming adjustment < 0 is a withdrawal and adjustment > 0 is a deposit)
        TRANSFER_IN,
        TRANSFER_OUT
    }
}
