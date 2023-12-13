using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL_Identity.Controllers
{

    [Authorize(Roles = "Usuario")]
    public class CarritoCompraController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public CarritoCompraController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index(string UserName)
        {
            string IdUsuario = BL.UserIdentity.GetByName(UserName);
            ML.Result result = BL.Carrito.GetCarrito(IdUsuario);
            ML.Carrito carrito = result.Correct? (ML.Carrito)result.Object : new ML.Carrito();
            return View(carrito);
        }
    }
}
