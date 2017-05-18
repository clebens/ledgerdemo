using ledgerdemo.Services.DBTableTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ledgerdemo.Services.User.DTOs
{
    public class UserViewModel : users {
        private new byte[] password { get { return null; } }
        private new byte[] salt { get { return null; } }
    }
}
