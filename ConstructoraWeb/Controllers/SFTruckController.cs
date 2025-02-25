using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.ViewModels;
using ConstructoraWeb.Models.Filters;

namespace ConstructoraWeb.Controllers
{
    [Session]
    public class SFTruckController : Controller
    {
        private readonly SFTruckSrv sfTruckSrv;
        private readonly SubModuleSrv _subModuleSrv;
        private readonly IConfiguration _configuration;
        private readonly string _imageStoragePath;

        public SFTruckController(IConfiguration configuration)
        {
            _configuration = configuration;
            sfTruckSrv = new SFTruckSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
            _subModuleSrv = new SubModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
            _imageStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        }

        public IActionResult Admon()
        {
            try
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                ViewBag.UserEmail = userEmail;
                bool hasPermission = _subModuleSrv.CheckPermissionOnView(userEmail, "SFTruck ", "Admon");
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
        public JsonResult ListTruck(SFTruckVM sfTruckVM)
        {
            return Json(sfTruckSrv.List(sfTruckVM));
        }

        [HttpPost]
        public JsonResult AddUpdate(SFTruckVM sfTruckVM, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/img", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                sfTruckVM.TruckImage = file.FileName;
            }

            var result = sfTruckSrv.AddUpdate(sfTruckVM);
            return Json(result);
        }

        [HttpPost]
        public JsonResult HandleStatus(int TruckID, string Action)
        {
            var sfTruckVM = new SFTruckVM
            {
                TruckID = TruckID,
                TruckSTS = (Action == "Activate") ? "Activo" : (Action == "Deactivate") ? "Inactivo" : ""
            };

            var result = sfTruckSrv.HandleStatus(sfTruckVM, Action);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteTruck(int TruckID)
        {
            var sfTruckVM = new SFTruckVM
            {
                TruckID = TruckID
            };
            var result = sfTruckSrv.DeleteTruck(sfTruckVM);

            return Json(result);
        }
    }
}
