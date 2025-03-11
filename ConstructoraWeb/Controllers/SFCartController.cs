using Microsoft.AspNetCore.Mvc;
using ConstructoraWeb.Models.Services;
using ConstructoraWeb.Models.Filters;
using ConstructoraWeb.Models.ViewModels;

namespace ConstructoraWeb.Controllers
{
    
    [Session]
    public class SFCartController : Controller
    {
        private readonly SFCartSrv _sfCartSrv;
        private readonly SubModuleSrv _subModuleSrv;
        private readonly IConfiguration _configuration;
        
        public SFCartController(IConfiguration configuration)
        {
            _configuration = configuration;
            _subModuleSrv = new SubModuleSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
            _sfCartSrv = new SFCartSrv(_configuration["ConnectionStrings:Cnx"] ?? "");
        }
    
        public IActionResult Admon()
        {
            try
            {
                var userName = HttpContext.Session.GetString("UserName");
                ViewBag.UserName = userName;
               
                var userEmail = HttpContext.Session.GetString("UserEmail");
                ViewBag.UserEmail = userEmail;
                bool hasPermission = _subModuleSrv.CheckPermissionOnView(userEmail, "SFCart ", "Admon");
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
        public IActionResult AddToCart(int productId, int quantity, int brandId)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                return Json(new { success = false, message = "Debes iniciar sesión para agregar productos al carrito." });
            }

            SFCartVM cartItem = new SFCartVM
            {
                id_producto = productId,
                cantidad = quantity,
                email_usuario = userEmail,
                id_marca = brandId
            };

            var response = _sfCartSrv.Add(cartItem);

            if (response != null && response.Ok)
            {
                return Json(new { success = true, message = response.Message });
            }
            else
            {
                return Json(new { success = false, message = response?.Message ?? "Error al agregar producto al carrito." });
            }
        }
    }
}