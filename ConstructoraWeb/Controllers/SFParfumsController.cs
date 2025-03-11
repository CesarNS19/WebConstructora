using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.Filters;

namespace ConstructoraWeb.Controllers
{
    
    [Session]
    public class SFParfumsController : Controller
    {
    private readonly SubModuleSrv _subModuleSrv;
    private readonly IConfiguration _configuration;
    public SFParfumsController(IConfiguration configuration)
    {
        _configuration = configuration;
        _subModuleSrv = new SubModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
    }
    
        public IActionResult Admon()
        {
            try
            {
                var userName = HttpContext.Session.GetString("UserName");
                ViewBag.UserName = userName;
               
                var userEmail = HttpContext.Session.GetString("UserEmail");
                ViewBag.UserEmail = userEmail;
                bool hasPermission = _subModuleSrv.CheckPermissionOnView(userEmail, "SFParfums ", "Admon");
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
    }
}