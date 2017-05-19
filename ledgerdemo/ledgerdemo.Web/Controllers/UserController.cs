using ledgerdemo.Service;
using ledgerdemo.Services.Account;
using ledgerdemo.Services.User;
using ledgerdemo.Services.User.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ledgerdemo.Web.Controllers {
    [RoutePrefix("API/User")]
    public class UserController : SharedController {
        private IAccountService AccountService;
        private IUserService UserService;

        public UserController(IAccountService accountService, IUserService userService) : base() {
            this.AccountService = accountService;
            this.UserService = userService;
        }

        
        private void addAuthCookie(string email) {
            var user = UserService.GetUserByEmail(email);
            var jsonUser = JsonConvert.SerializeObject(user);
            string encryptedJsonUser;
            
            using (var aes = Aes.Create() ) {
                aes.GenerateIV();
                aes.Key = Convert.FromBase64String(GlobalConstants.AUTH_TOKEN_GEN_KEY);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                var toEncryptBytes = Encoding.UTF8.GetBytes(jsonUser);
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV)) {
                    using (var ms = new MemoryStream()) {
                        ms.Write(aes.IV, 0, 16);
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                            cs.Write(toEncryptBytes, 0, toEncryptBytes.Length);
                            cs.FlushFinalBlock();
                        }
                        encryptedJsonUser = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            var cookie = new HttpCookie(GlobalConstants.AUTH_COOKIE_NAME, encryptedJsonUser);
            cookie.Expires = DateTime.Now.AddDays(30);
            Response.AppendCookie(cookie);
        }

        [Route("CreateAccount")]
        [HttpPost]
        public ActionResult CreateAccount(string email, string password) {
            try {
                var user = UserService.CreateUser(new CreateUserModel {
                    email = email,
                    password = password
                });
                AccountService.CreateAccountForUser(user.userid);
                addAuthCookie(email);
                return Json(new { success = true });
            } catch (DisplayedException ex) {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Route("Login")]
        [HttpPost]
        public ActionResult Login(string email, string password) {
            if (UserService.AuthenticateUser(email, password)) {
                addAuthCookie(email);
                return Redirect("/");
            }
            return new HttpUnauthorizedResult();
        }

        [Route("Logout")]
        [HttpGet]
        public ActionResult Logout() {
            removeUserCookie();
            return Redirect("/");
        }
    }
}