using ledgerdemo.Services.User.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ledgerdemo.Services.DBTableTypes;

namespace ledgerdemo.Services.Test.User.Mocks {
    public class MockUserRepository : IUserRepository {
        public List<users> users { get; set; }

        public MockUserRepository() {
            this.users = new List<users>();
        }

        public users CreateUser(users user) {
            if (user.userid != 0) throw new Exception("UserRepository: Created user must have empty userid.");
            if (users.Any(x => x.email == user.email)) throw new Exception("UserRepository: User with email already exists.");
            user.userid = (users.Count > 0) ? users.Max(x => x.userid) + 1 : 1;
            users.Add(user);
            return user;
        }

        public users GetUserByEmail(string email) {
            return users.FirstOrDefault(x => x.email == email);
        }
    }
}
