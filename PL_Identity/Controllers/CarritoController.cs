using Microsoft.AspNetCore.Mvc;

namespace PL_Identity.Controllers
{
    public class CarritoController : Controller
    {
        public IActionResult VistaCarrito()
        {
            return View();
        }
    }
}
