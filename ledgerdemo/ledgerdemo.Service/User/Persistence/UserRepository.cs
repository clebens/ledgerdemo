using ledgerdemo.Service;
using ledgerdemo.Services.DBTableTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ledgerdemo.Services.User.Persistence
{
    public interface IUserRepository {
        users GetUserByEmail(string email);
        users CreateUser(users user);
    }
    public class UserRepository : IUserRepository {
        private IDB DB;
        public UserRepository(IDB db) {
            this.DB = db;
        }

        public users CreateUser(users user) {
            if (user.userid != 0) throw new Exception("UserRepository: Created user must have empty userid.");
            if (DB.users.Any(x => x.email == user.email)) throw new DisplayedException("User with email already exists.");
            user.userid = (DB.users.Count > 0) ? DB.users.Max(x => x.userid) + 1 : 1;
            DB.users.Add(user);
            return user;
        }

        public users GetUserByEmail(string email) {
            return DB.users.FirstOrDefault(x => x.email == email);
        }
    }
}
