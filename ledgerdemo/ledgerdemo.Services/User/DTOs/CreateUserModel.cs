using System;
using System.Collections.Generic;
using System.Text;

namespace ledgerdemo.Services.User.DTOs
{
    public class CreateUserModel {
        public string email { get; set; }
        public string password { get; set; }
    }
}
