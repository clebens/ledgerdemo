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

        private readonly int PW_ITERATION_COUNT = 10000;
        private readonly int PW_LENGTH = 32;

        private PwObj createpw(string pw) {

            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(pw, 32);
            rfc2898DeriveBytes.IterationCount = PW_ITERATION_COUNT;
            return new PwObj {
                hash = rfc2898DeriveBytes.GetBytes(PW_LENGTH),
                salt = rfc2898DeriveBytes.Salt
            };
        }

        private bool ValidatePassword(string pw, byte[] salt, byte[] testHash) {
            Rfc2898DeriveBytes r2db = new Rfc2898DeriveBytes(pw, salt, PW_ITERATION_COUNT);
            var inputhash = r2db.GetBytes(PW_LENGTH);

            if (testHash.Length != inputhash.Length) return false;
            for (var i = 0; i < testHash.Length; i++) {
                if (testHash[i] != inputhash[i]) return false;
            }
            return true;
        }

        public bool AuthenticateUser(string email, string password) {
            var dbuser = UserRepository.GetUserByEmail(email);
            return ValidatePassword(password, dbuser.salt, dbuser.password);
        }

        public UserViewModel CreateUser(CreateUserModel user) {
            var newpw = createpw(user.password);
            var dbuser = UserRepository.CreateUser(new users {
                userid = 0,
                email = user.email,
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
