using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Filters;
namespace ConstructoraWeb.Controllers
{
    [Session]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewBag.UserName = userName;
            return View();
        }
    }
}
