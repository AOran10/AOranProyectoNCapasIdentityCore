using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ML;

namespace PL_Identity.Controllers
{

    [Authorize(Roles = "Usuario")]
    public class CarritoCompraController : Controller
    {
        public IActionResult Index(string UserName)
        {
            ML.Result result = BL.Carrito.GetCarrito(UserName);
            ML.Carrito carrito = result.Correct? (ML.Carrito)result.Object : new ML.Carrito();
            return View(carrito);
        }
        [Authorize(Roles = "Usuario")]
        public IActionResult AddToCar(string Orden)
        {
            string[] OrdenList = Orden.Split(',');
            ML.Orden orden = new ML.Orden();
            orden.Usuario = new ML.UserIdentity();
            orden.Usuario.UserName = OrdenList[0];
            orden.Cantidad = int.Parse(OrdenList[1]);
            orden.Producto = new ML.Producto();
            orden.Producto.Id = int.Parse(OrdenList[2]);
            orden.Producto.Precio = decimal.Parse(OrdenList[3]);
            
            ML.Result result = BL.Carrito.AddToCar(orden);
            ViewBag.Mensaje = result.ErrorMessage;
            ViewBag.Subtitle = "Añadir carrito";
            return View("Modal");
        }
        public IActionResult Delete(int IdDetalle)
        {
            Result result = BL.Carrito.Delete(IdDetalle);
            ViewBag.Mensaje = result.ErrorMessage;
            ViewBag.Subtitle = "Añadir carrito";
            return View("Modal");
        }
        
        public IActionResult AddPedido(int IdCarrito)
        {
            ML.Result result = BL.Pedido.AddPedido(IdCarrito);
            ViewBag.Mensaje = result.ErrorMessage;
            ViewBag.Subtitle = "Realizar Pedido";
            return View("Modal");
        }
    }
}
