using ledgerdemo.Services.Account;
using ledgerdemo.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ledgerdemo.Web.Controllers {
    [RoutePrefix("API/Account")]
    public class AccountController : SharedController {
        private IAccountService AccountService;
        private IUserService UserService;

        public AccountController(IAccountService accountService, IUserService userService) : base() {
            this.AccountService = accountService;
            this.UserService = userService;
        }

        private bool checkUserAccountAuthorization(int accountid) {
            var user = getUserCookie();
            if (user == null) return false;
            var account = AccountService.GetAccountForUser(user.userid);
            if (accountid != account.accountid) return false;
            return true;
        }

        [Route("{accountid}/deposit")]
        public ActionResult Deposit(int accountid, int amount) {
            checkUserAccountAuthorization(accountid);
            AccountService.Deposit(accountid, amount);
            return Json(new { success = true, account = AccountService.GetAccount(accountid) });
        }

        [Route("{accountid}/withdraw")]
        public ActionResult Withdraw(int accountid, int amount) {
            checkUserAccountAuthorization(accountid);
            AccountService.Withdraw(accountid, amount);
            return Json(new { success = true, account = AccountService.GetAccount(accountid) });
        }

        [HttpGet]
        [Route("{accountid}/transactions")]
        public ActionResult Transactions(int accountid) {
            checkUserAccountAuthorization(accountid);
            return Json(AccountService.GetTransactionLogForAccount(accountid), JsonRequestBehavior.AllowGet);
        }

        [Route("user/{userid}")]
        public ActionResult UserAccountDetails(int userid) {
            return Json(AccountService.GetAccountForUser(userid), JsonRequestBehavior.AllowGet);
        }
    }
}