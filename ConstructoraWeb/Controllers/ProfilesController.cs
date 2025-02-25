using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.ViewModels;
using ConstructoraWeb.Models.Filters;

namespace ConstructoraWeb.Controllers
{
    [Session]
    public class ProfilesController : Controller
    {
        private readonly ProfileSrv _profileSrv;
        private readonly SubModuleSrv _subModuleSrv;
        private readonly IConfiguration _configuration;

        public ProfilesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _subModuleSrv = new SubModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
            _profileSrv = new ProfileSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
        }

        public IActionResult Admon()
        {
            try
            {

                var userEmail = HttpContext.Session.GetString("UserEmail");
                ViewBag.UserEmail = userEmail;
                bool hasPermission = _subModuleSrv.CheckPermissionOnView(userEmail, "Profiles ", "Admon");
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
        public JsonResult List(ProfileVM profileVM)
        {
            return Json(_profileSrv.List(profileVM));
        }

        [HttpPost]
        public JsonResult AddUpdate(ProfileVM profileVM)
        {
            return Json(_profileSrv.AddUpdate(profileVM));
        }

        [HttpPost]
        public JsonResult HandleStatus(int ProfileID, string Action)
        {
            var profileVM = new ProfileVM
            {
                ProfileID = ProfileID,
                ProStatus = (Action == "Activate") ? "Activo" : (Action == "Deactivate") ? "Inactivo" : ""
            };

            var result = _profileSrv.HandleStatus(profileVM, Action);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteProfile(int ProfileID)
        {
            var profileVM = new ProfileVM
            {
                ProfileID = ProfileID
            };
            var result = _profileSrv.DeleteProfile(profileVM);

            return Json(result);
        }
    }
}
