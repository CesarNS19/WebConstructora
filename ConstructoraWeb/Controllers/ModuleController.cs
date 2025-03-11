using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.ViewModels;
using ConstructoraWeb.Models.Filters;

namespace ConstructoraWeb.Controllers
{
  [Session]
  public class ModuleController : Controller
  {
    private readonly ModuleSrv moduleSrv;
    private readonly SubModuleSrv _subModuleSrv;
    private readonly IConfiguration _configuration;
    public ModuleController(IConfiguration configuration)
    {
      _configuration = configuration;
      _subModuleSrv =new SubModuleSrv(_configuration["ConnectionStrings:Cnx"]??"");
      moduleSrv = new ModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
    }
    public IActionResult Admon()
    {
      try
            {
                var userName = HttpContext.Session.GetString("UserName");
                ViewBag.UserName = userName;
                
                var userEmail = HttpContext.Session.GetString("UserEmail");
                ViewBag.UserEmail = userEmail;
                bool hasPermission = _subModuleSrv.CheckPermissionOnView(userEmail, "Module ", "Admon");
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
    public JsonResult ListModule(ModuleVM moduleVM)
    {
      return Json(moduleSrv.List(moduleVM));
    }

        [HttpPost]
        public JsonResult AddUpdate(ModuleVM moduleVM)
        {
            return Json(moduleSrv.AddUpdate(moduleVM));
        }

        [HttpPost]
        public JsonResult ActivateStatus(int ModuleID)
        {
            var moduleVM = new ModuleVM
            {
                ModuleID = ModuleID
            };
            var result = moduleSrv.ActivateStatus(moduleVM);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeactivateStatus(int ModuleID)
        {
            var moduleVM = new ModuleVM
            {
                ModuleID = ModuleID
            };
            var result = moduleSrv.DeactivateStatus(moduleVM);

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpModule(int ModuleID)
        {
            var moduleVM = new ModuleVM
            {
                ModuleID = ModuleID
            };
            var result = moduleSrv.UpModule(moduleVM);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DownModule(int ModuleID)
        {
            var moduleVM = new ModuleVM
            {
                ModuleID = ModuleID
            };
            var result = moduleSrv.DownModule(moduleVM);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteModule(int ModuleID)
        {
            var moduleVM = new ModuleVM
            {
                ModuleID = ModuleID
            };
            var result = moduleSrv.DeleteModule(moduleVM);

            return Json(result);
        }
    }
}
