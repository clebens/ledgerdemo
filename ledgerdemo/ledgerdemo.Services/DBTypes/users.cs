using System;
using System.Collections.Generic;
using System.Text;

namespace ledgerdemo.Services.DBTableTypes
{
    public class users {
        public int userid { get; set; }
        public string email { get; set; }
        public byte[] password { get; set; }
        public byte[] salt { get; set; }
    }
}
