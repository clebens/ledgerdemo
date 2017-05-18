using ledgerdemo.Services.DBTableTypes;
using ledgerdemo.Services.User.DTOs;
using ledgerdemo.Services.User.Persistence;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ledgerdemo.Services.User
{
    public interface IUserService {
        UserViewModel CreateUser(CreateUserModel user);
        UserViewModel GetUserByEmail(string email); 
        bool AuthenticateUser(string email, string password);
    }

    public class UserService : IUserService {
        private IUserRepository UserRepository;

        public UserService(IUserRepository userRepository) {
            this.UserRepository = userRepository;
        }

        public UserViewModel GetUserByEmail(string email) {
            var dbuser = UserRepository.GetUserByEmail(email);
            return new UserViewModel {
                email = dbuser.email,
                userid = dbuser.userid
            };
        }

        private class PwObj {
            public byte[] hash;
            public byte[] salt;
        }
        private PwObj createpw(string pw) {

            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(pw, 32);
            rfc2898DeriveBytes.IterationCount = 10000;
            return new PwObj {
                hash = rfc2898DeriveBytes.GetBytes(20),
                salt = rfc2898DeriveBytes.Salt
            };
        }

        public bool AuthenticateUser(string email, string password) {
            throw new NotImplementedException();
        }

        public UserViewModel CreateUser(CreateUserModel user) {
            var newpw = createpw(user.password);
            var dbuser = UserRepository.CreateUser(new users {
                password = newpw.hash,
                salt = newpw.salt
            });

            return new UserViewModel {
                userid = dbuser.userid,
                email = dbuser.email
            };

        }
    }
}
