using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.ViewModels;
using ConstructoraWeb.Models.Filters;

namespace ConstructoraWeb.Controllers
{
  [Session]
  public class SFBrandsController : Controller
  {
    private readonly SFBrandsSrv _sfBrandsSrv;
    private readonly SubModuleSrv _subModuleSrv;
    private readonly IConfiguration _configuration;
    public SFBrandsController(IConfiguration configuration)
    {
      _configuration = configuration;
      _subModuleSrv = new SubModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
      _sfBrandsSrv = new SFBrandsSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
    }
    
    public IActionResult Admon()
    {
      try
      {
        var userName = HttpContext.Session.GetString("UserName");
        ViewBag.UserName = userName;
        
        var userEmail = HttpContext.Session.GetString("UserEmail");
        ViewBag.UserEmail = userEmail;
        bool hasPermission = _subModuleSrv.CheckPermissionOnView(userEmail, "SFBrands ", "Admon");
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
    public JsonResult ListBrand(SFBrandsVM sfBrandsVm)
    {
      return Json(_sfBrandsSrv.List(sfBrandsVm));
    }

    [HttpPost]
    public JsonResult AddUpdate(SFBrandsVM sfBrandsVm)
    {
      return Json(_sfBrandsSrv.AddUpdate(sfBrandsVm));
    }
    
    [HttpPost]
    public JsonResult HandleStatus(int id_marca, string Action)
    {
      var sfBrandsVM = new SFBrandsVM
      {
        id_marca = id_marca,
        estatus = (Action == "Activate") ? "Activo" :
                     (Action == "Deactivate") ? "Inactivo" : ""
      };

      var result = _sfBrandsSrv.HandleStatus(sfBrandsVM, Action);

      return Json(result);
    }
  }
}