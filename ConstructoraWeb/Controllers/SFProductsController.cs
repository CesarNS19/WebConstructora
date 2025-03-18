using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.ViewModels;
using ConstructoraWeb.Models.Filters;

namespace ConstructoraWeb.Controllers
{
  [Session]
  public class SFProductsController : Controller
  {
    private readonly SFProductsSrv _sfProductsSrv;
    private readonly SubModuleSrv _subModuleSrv;
    private readonly IConfiguration _configuration;
    public SFProductsController(IConfiguration configuration)
    {
      _configuration = configuration;
      _subModuleSrv = new SubModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
      _sfProductsSrv = new SFProductsSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
    }

    public IActionResult Admon()
    {
     try
            {
              var userName = HttpContext.Session.GetString("UserName");
              ViewBag.UserName = userName;

                var userEmail = HttpContext.Session.GetString("UserEmail");
                ViewBag.UserEmail = userEmail;
                bool hasPermission = _subModuleSrv.CheckPermissionOnView(userEmail, "SFProducts ", "Admon");
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
    public JsonResult ListParfum(SFProductsVM sfProductsVm)
    {
      return Json(_sfProductsSrv.List(sfProductsVm));
    }
    
    [HttpPost]
    public JsonResult ListProducts(SFProductsVM sfProductsVm)
    {
      return Json(_sfProductsSrv.ListProducts(sfProductsVm));
    }

    [HttpPost]
    public JsonResult AddUpdate(SFProductsVM sfProductsVm, IFormFile file)
    {
      if (file != null && file.Length > 0)
      {
        var filePath = Path.Combine("wwwroot/img", file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          file.CopyTo(stream);
        }

        sfProductsVm.imagen = file.FileName;
      }

      var result = _sfProductsSrv.AddUpdate(sfProductsVm);
      return Json(result);
    }
    
    [HttpPost]
    public JsonResult HandleStatus(int id_producto, string Action)
    {
      var sfProductsVm = new SFProductsVM
      {
        id_producto = id_producto,
        estatus = (Action == "Activate") ? "Activo" :
                     (Action == "Deactivate") ? "Pendiente" : ""
      };

      var result = _sfProductsSrv.HandleStatus(sfProductsVm, Action);

      return Json(result);
    }
    
    [HttpPost]
    public JsonResult DeleteParfum(int id_producto)
    {
      var sfProductsVm = new SFProductsVM
      {
        id_producto = id_producto
      };
      var result = _sfProductsSrv.DeleteParfum(sfProductsVm);

      return Json(result);
    }
  }
}