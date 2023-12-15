using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL_Identity.Controllers
{
    public class PedidoAdminController : Controller
    {
        [Authorize(Roles = "Administrador")]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Pedido.GetAllAdmin();
            ML.Result resultEstatus = BL.Estatus.GetAll();

            ML.Pedido pedido = new ML.Pedido();
            pedido.Pedidos = result.Objects;
            pedido.Estatus = new ML.Estatus();
            pedido.Estatus.EstatusList = resultEstatus.Objects;

            return View(pedido);
        }
        public JsonResult UpdateStatus(int IdPedido, int IdEstatus)
        {
            ML.Result result = BL.Estatus.UpdateEstatus(IdPedido, IdEstatus);

            return Json(result);
        }
    }
}
