using ledgerdemo.Services.Test.User.Mocks;
using ledgerdemo.Services.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ledgerdemo.Services.Test.User {
    [TestClass]
    public class UserServiceTests {
        private UserService SUT;
        private MockUserRepository repo;

        [TestInitialize]
        public void Setup() {
            this.repo = new MockUserRepository();
            this.SUT = new UserService(repo);
        }
        [TestMethod]
        public void ShouldAllowLoginWithSamePassword() {
            SUT.CreateUser(new Services.User.DTOs.CreateUserModel {
                email = "test@testuser.com",
                password = "testpassword"
            });

            var result = SUT.AuthenticateUser("test@testuser.com", "testpassword");
        }

        [TestMethod]
        public void ShouldNotAllowLoginWithSamePassword() {
            SUT.CreateUser(new Services.User.DTOs.CreateUserModel {
                email = "test@testuser.com",
                password = "testpassword"
            });

            var result = SUT.AuthenticateUser("test@testuser.com", "badpassword");
        }
    }
}
