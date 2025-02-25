using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.ViewModels;
using ConstructoraWeb.Models.Filters;

namespace ConstructoraWeb.Controllers
{
    [Session]
    public class SubModuleController : Controller
    {
        private readonly SubModuleSrv submoduleSrv;
        private readonly IConfiguration _configuration;
        public SubModuleController(IConfiguration configuration)
        {
            _configuration = configuration;
            submoduleSrv = new SubModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
        }

        [HttpPost]
        public JsonResult ListSubModule(SubModuleVM submoduleVM)
        {
            return Json(submoduleSrv.List(submoduleVM));
        }

        [HttpPost]
        public JsonResult AddUpdate(SubModuleVM submoduleVM)
        {
            return Json(submoduleSrv.AddUpdate(submoduleVM));
        }
        [HttpPost]
        public JsonResult ListPermission(long auxID)
        {

            return Json(submoduleSrv.ListPermission(auxID));
        }

        [HttpPost]
        public JsonResult ListModuleOnProfile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var submoduleVM = new SubModuleVM
            {
                AcEmailAddress = email
            };
            return Json(submoduleSrv.ListModuleOnProfile(submoduleVM));
        }

        [HttpPost]
        public JsonResult UpSubModule(int SubModuleID)
        {
            var subModuleVM = new SubModuleVM
            {
                SubModuleID = SubModuleID
            };
            var result = submoduleSrv.UpSubModule(subModuleVM);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DownSubModule(int SubModuleID)
        {
            var subModuleVM = new SubModuleVM
            {
                SubModuleID = SubModuleID
            };
            var result = submoduleSrv.DownSubModule(subModuleVM);

            return Json(result);
        }

        [HttpPost]
        public JsonResult ActivateStatus(int SubModuleID)
        {
            var subModuleVM = new SubModuleVM
            {
                SubModuleID = SubModuleID
            };
            var result = submoduleSrv.ActivateStatus(subModuleVM);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeactivateStatus(int SubModuleID)
        {
            var subModuleVM = new SubModuleVM
            {
                SubModuleID = SubModuleID
            };
            var result = submoduleSrv.DeactivateStatus(subModuleVM);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteSubModule(int SubModuleID)
        {
            var subModuleVM = new SubModuleVM
            {
                SubModuleID = SubModuleID
            };
            var result = submoduleSrv.DeleteSubModule(subModuleVM);

            return Json(result);
        }
    }
}