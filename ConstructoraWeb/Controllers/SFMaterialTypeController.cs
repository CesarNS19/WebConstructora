using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.ViewModels;
using ConstructoraWeb.Models.Filters;

namespace ConstructoraWeb.Controllers
{
  [Session]
  public class SFMaterialTypeController : Controller
  {
    private readonly SFMaterialTypeSrv sfMaterialTypeSrv;
    private readonly IConfiguration _configuration;
    public SFMaterialTypeController(IConfiguration configuration)
    {
      _configuration = configuration;
      sfMaterialTypeSrv = new SFMaterialTypeSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
    }
    
    [HttpPost]
    public JsonResult ListMaterial(SFMaterialTypeVM sfMaterialTypeVM)
    {
      return Json(sfMaterialTypeSrv.List(sfMaterialTypeVM));
    }

    [HttpPost]
    public JsonResult AddUpdate(SFMaterialTypeVM sfMaterialTypeVM)
    {
      return Json(sfMaterialTypeSrv.AddUpdate(sfMaterialTypeVM));
    }
        [HttpPost]
    public JsonResult HandleStatus(int TypeMeterialID, string Action)
    {
      var sfMaterialTypeVM = new SFMaterialTypeVM
      {
        TypeMeterialID = TypeMeterialID,
        TypeMeterialSTS = (Action == "Activate") ? "Activo" :
                     (Action == "Deactivate") ? "Pendiente" : ""
      };

      var result = sfMaterialTypeSrv.HandleStatus(sfMaterialTypeVM, Action);

      return Json(result);
    }
  }
}