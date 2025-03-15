using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.Filters;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Controllers
{

    [Session]
    public class SFCartController : Controller
    {
        private readonly SFCartSrv _sfCartSrv;
        private readonly SubModuleSrv _subModuleSrv;
        private readonly IConfiguration _configuration;

        public SFCartController(IConfiguration configuration)
        {
            _configuration = configuration;
            _subModuleSrv = new SubModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
            _sfCartSrv = new SFCartSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
        }

        public IActionResult Admon()
        {
            try
            {
                var userName = HttpContext.Session.GetString("UserName");
                ViewBag.UserName = userName;

                var userEmail = HttpContext.Session.GetString("UserEmail");
                ViewBag.UserEmail = userEmail;
                bool hasPermission = _subModuleSrv.CheckPermissionOnView(userEmail, "SFCart ", "Admon");
                if (hasPermission)
                {
                    return View();
                }

                return RedirectToAction("Auth", "Account");


            }
            catch (Exception)
            {
                return RedirectToAction("Auth", "Account");
            }
        }

        [HttpPost]
        public JsonResult ListCart(SFCartVM sfCartVm)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                return null;
            }

            sfCartVm.email_usuario = userEmail;

            return Json(_sfCartSrv.List(sfCartVm));
        }

    [HttpPost]
        public JsonResult Add([FromBody] SFCartVM sfCartVM)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                return null;
            }
            sfCartVM.email_usuario = userEmail;

            var result = _sfCartSrv.Add(sfCartVM);

            return Json(result);
        }
        
        [HttpPost]
        public JsonResult resCant(SFCartVM sfCartVM)
        {
            var result = _sfCartSrv.ResCant(sfCartVM);

            return Json(result);
        }
        
        [HttpPost]
        public JsonResult AddCant(SFCartVM sfCartVM)
        {
            var result = _sfCartSrv.AddCant(sfCartVM);

            return Json(result);
        }
        
        [HttpPost]
        public JsonResult deleteCart(SFCartVM sfCartVM)
        {
            var result = _sfCartSrv.DeleteCart(sfCartVM);

            return Json(result);
        }
    }
}