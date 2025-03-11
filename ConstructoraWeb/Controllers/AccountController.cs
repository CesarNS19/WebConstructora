using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Controllers
{

    public class AccountController : Controller
    {
        private readonly AccountSrv _accountSrv;
        private readonly SubModuleSrv _subModuleSrv;
        private readonly IConfiguration _configuration;
        private readonly string _salt;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _subModuleSrv = new SubModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
            _accountSrv = new AccountSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
            _salt = _configuration["AppSettings:Salt"] ?? "";
        }

        public IActionResult Auth()
        {
            return View();
        }

        public IActionResult Admon()
        {
            try
            {
                var userName = HttpContext.Session.GetString("UserName");
                ViewBag.UserName = userName;
                
                var userEmail = HttpContext.Session.GetString("UserEmail");
                ViewBag.UserEmail = userEmail;
                bool hasPermission = _subModuleSrv.CheckPermissionOnView(userEmail, "Account", "Admon");
                if (hasPermission)
                {
                    return View();
                }
                HttpContext.Session.Clear();
            return RedirectToAction("Auth", "Account");


            }
            catch (Exception)
            {
                HttpContext.Session.Clear();
            return RedirectToAction("Auth", "Account");
            }
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                password = Encrypt.GetSHA256(password, _salt);
            }

            ResponseVM<string> response = _accountSrv.Login(email, password);

            if (response.Ok)
            {
                HttpContext.Session.SetString("UserEmail", email);
                HttpContext.Session.SetString("UserName", response.Data);
                return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
            }
            else
            {
                return Json(new { success = false, message = response.Message });
            }
        }

        [HttpPost]
        public JsonResult List(AccountVM accountVM)
        {
            return Json(_accountSrv.List(accountVM));
        }

        [HttpPost]
        public JsonResult AddUpdate(AccountVM accountVM)
        {
            if (!string.IsNullOrEmpty(accountVM.AcPassword))
            {
                accountVM.AcPassword = Encrypt.GetSHA256(accountVM.AcPassword, _salt);
            }

            return Json(_accountSrv.AddUpdate(accountVM));
        }

        [HttpPost]
        public JsonResult HandleStatus(int AccountID, string Action)
        {
            var accountVM = new AccountVM
            {
                AccountID = AccountID,
                AcStatus = (Action == "Activate") ? "Activo" :
                           (Action == "Deactivate") ? "Pendiente" : ""
            };

            var result = _accountSrv.HandleStatus(accountVM, Action);

            return Json(result);
        }


        [HttpPost]
        public JsonResult ChangePass(int AccountID, string AcPassword)
        {
            if (!string.IsNullOrEmpty(AcPassword))
            {
                AcPassword = Encrypt.GetSHA256(AcPassword, _salt);
            }
            var accountVM = new AccountVM
            {
                AccountID = AccountID,
                AcPassword = AcPassword
            };

            return Json(_accountSrv.ChangePass(accountVM));
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Auth", "Account");
        }
    }
}
