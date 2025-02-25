using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.ViewModels;
using ConstructoraWeb.Models.Filters;

namespace ConstructoraWeb.Controllers
{
  [Session]
  public class SFServicesController : Controller
  {
    private readonly SFServicesSrv _sfServicesSrv;
    private readonly SubModuleSrv _subModuleSrv;
    private readonly IConfiguration _configuration;
    public SFServicesController(IConfiguration configuration)
    {
      _configuration = configuration;
      _subModuleSrv = new SubModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
      _sfServicesSrv = new SFServicesSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
    }

    public IActionResult Admon()
    {
     try
            {

                var userEmail = HttpContext.Session.GetString("UserEmail");
                ViewBag.UserEmail = userEmail;
                bool hasPermission = _subModuleSrv.CheckPermissionOnView(userEmail, "SFServices ", "Admon");
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
    public JsonResult ListMaterial(SFservicesVM sFservicesVm)
    {
      return Json(_sfServicesSrv.List(sFservicesVm));
    }

    [HttpPost]
    public JsonResult AddUpdate(SFservicesVM sFservicesVm, IFormFile file)
    {
      if (file != null && file.Length > 0)
      {
        var filePath = Path.Combine("wwwroot/img", file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          file.CopyTo(stream);
        }

        sFservicesVm.MeterialImage = file.FileName;
      }

      var result = _sfServicesSrv.AddUpdate(sFservicesVm);
      return Json(result);
    }
    [HttpPost]
    public JsonResult HandleStatus(int MeterialID, string Action)
    {
      var sfMaterialVM = new SFservicesVM
      {
        MeterialID = MeterialID,
        MeterialSTS = (Action == "Activate") ? "Activo" :
                     (Action == "Deactivate") ? "Pendiente" : ""
      };

      var result = _sfServicesSrv.HandleStatus(sfMaterialVM, Action);

      return Json(result);
    }

  }
}