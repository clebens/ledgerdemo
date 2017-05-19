﻿using ledgerdemo.Services;
using ledgerdemo.Services.Account;
using ledgerdemo.Services.User;
using ledgerdemo.Services.User.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ledgerdemo.Web.Controllers {
    public class ViewsController : SharedController {
        private IAccountService AccountService;
        private IUserService UserService;

        public ViewsController(IAccountService accountService, IUserService userService) : base() {
            this.AccountService = accountService;
            this.UserService = userService;
        }


        [Route()]
        [Route("Index")]
        public ActionResult Index() {
            if (getUserCookie() != null) return Redirect("/Account");
            return View();
        }

        [Route("CreateAccount")]
        public ActionResult CreateAccount() {
            return View();
        }

        [Route("Account")]
        public ActionResult Account() {
            var user = getUserCookie();
            if (user == null || UserService.GetUserByEmail(user.email) == null) {
                removeUserCookie();
                return Redirect("/");
            }
            ViewData["userid"] = user.userid;
            return View();
        }

        [Route("Login")]
        public ActionResult Login() {
            if (getUserCookie() != null) return Redirect("/Account");
            return View();
        }

        [Route("Logout")]
        public ActionResult Logout() {
            return Redirect("API/user/logout");
        }
    }
}