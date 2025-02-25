using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.ViewModels;
using ConstructoraWeb.Models.Filters;

namespace ConstructoraWeb.Controllers
{
  [Session]
  public class SFTruckTypeController : Controller
  {
    private readonly SFTruckTypeSrv sfTruckTypeSrv;
    private readonly IConfiguration _configuration;
    public SFTruckTypeController(IConfiguration configuration)
    {
      _configuration = configuration;
      sfTruckTypeSrv = new SFTruckTypeSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
    }

    [HttpPost]
    public JsonResult ListTruckType(SFTruckTypeVM sfTruckTypeVM)
    {
      return Json(sfTruckTypeSrv.List(sfTruckTypeVM));
    }

    [HttpPost]
    public JsonResult AddUpdate(SFTruckTypeVM sfTruckTypeVM)
    {
      return Json(sfTruckTypeSrv.AddUpdate(sfTruckTypeVM));
    }

    [HttpPost]
    public JsonResult HandleStatus(int TypeID, string action)
    {
      var sFTruckTypeVM = new SFTruckTypeVM
      {
        TypeID = TypeID,
        TypeSTS = (action == "Activate") ? "Activo" : (action == "Deactivate") ? "Inactivo" : ""
      };

      var result = sfTruckTypeSrv.HandleStatus(sFTruckTypeVM, action);

      return Json(result);
    }

    [HttpPost]
        public JsonResult DeleteTruckType(int TypeID)
        {
            var sfTruckTypeVM = new SFTruckTypeVM
            {
                TypeID = TypeID
            };
            var result = sfTruckTypeSrv.DeleteTruckType(sfTruckTypeVM);

            return Json(result);
        }
  }
}