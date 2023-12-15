using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL_Identity.Controllers
{
    [Authorize(Roles = "Usuario")]
    public class PedidoUserController : Controller
    {
        public IActionResult Index(string UserName)
        {
            ML.Result result = BL.Pedido.GetPedidos(UserName);
            ML.Pedido pedido = new ML.Pedido();

            pedido.Pedidos = result.Objects;
            return View(pedido);
        }
    }
}
