using ledgerdemo.Services.User.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ledgerdemo.Web.Controllers {
    public class SharedController : Controller {
        public SharedController() : base() {
        } 

        protected UserViewModel LoggedInUser { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            LoggedInUser = getUserCookie();
            ViewBag.LoggedInUser = LoggedInUser;
        }

        protected void removeUserCookie() {
            var c = new HttpCookie(GlobalConstants.AUTH_COOKIE_NAME);
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);
        }

        private UserViewModel getUserCookie() {
            try {
                var authtoken = Request.Cookies.Get(GlobalConstants.AUTH_COOKIE_NAME);
                if (authtoken == null) return null;

                var encryptedUser = Convert.FromBase64String(authtoken.Value);

                string decryptedUser;

                using (var provider = Aes.Create()) {
                    provider.Key = Convert.FromBase64String(GlobalConstants.AUTH_TOKEN_GEN_KEY);
                    provider.Mode = CipherMode.CBC;
                    provider.Padding = PaddingMode.PKCS7;
                    using (var ms = new MemoryStream(encryptedUser)) {
                        byte[] buffer = new byte[16];
                        ms.Read(buffer, 0, 16);
                        provider.IV = buffer;
                        using (var decryptor = provider.CreateDecryptor(provider.Key, provider.IV)) {
                            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                                byte[] decrypted = new byte[encryptedUser.Length];
                                var byteCount = cs.Read(decrypted, 0, encryptedUser.Length);
                                decryptedUser = Encoding.UTF8.GetString(decrypted, 0, byteCount);
                            }
                        }
                    }
                }
                
                return JsonConvert.DeserializeObject<UserViewModel>(decryptedUser);
            } catch {
                removeUserCookie();
                return null;
            }
        }

    }
}