using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ML;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;

namespace PL_Identity.Controllers
{

    [Authorize(Roles = "Usuario")]
    public class CarritoCompraController : Controller
    {
        public async Task<IActionResult> Index(string UserName)
        {
            ML.Result result = await CarritoGet(UserName);
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
        //---------------------------------------------
        private async Task<ML.Result> CarritoGet(string UserName)
        {
            ML.Result result = new ML.Result();
            try
            {
                var options = new RestClientOptions("http://localhost:5286/api/Carrito/get/" + UserName);
                var client = new RestClient(options);
                var request = new RestRequest("");
                var response = await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ML.Result preresult = System.Text.Json.JsonSerializer.Deserialize<ML.Result>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


                     string objparticular = preresult.Object.ToString();
                    ML.Carrito resultobject = System.Text.Json.JsonSerializer.Deserialize<ML.Carrito>(objparticular, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var copia = resultobject.Carritos.ToList();
                    resultobject.Carritos = new List<object>();
                    foreach (var item in copia)
                    {
                        ML.DetalleCarrito resultDetalle = System.Text.Json.JsonSerializer.Deserialize<ML.DetalleCarrito>(item.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        resultobject.Carritos.Add(resultDetalle);
                    }
                    result = preresult;
                    result.Object = resultobject;
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }
        private async Task<ML.Result> Add(ML.Orden orden)
        {
            ML.Result result = new ML.Result();
            string consult = JsonConvert.SerializeObject(orden);
            try
            {
                var options = new RestClientOptions("http://localhost:5286/api/Carrito/add/" );
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddJsonBody(consult, false);
                var response = await client.PostAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ML.Result preresult = System.Text.Json.JsonSerializer.Deserialize<ML.Result>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


                    string objparticular = preresult.Object.ToString();
                    ML.Carrito resultobject = System.Text.Json.JsonSerializer.Deserialize<ML.Carrito>(objparticular, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var copia = resultobject.Carritos.ToList();
                    resultobject.Carritos = new List<object>();
                    foreach (var item in copia)
                    {
                        ML.DetalleCarrito resultDetalle = System.Text.Json.JsonSerializer.Deserialize<ML.DetalleCarrito>(item.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        resultobject.Carritos.Add(resultDetalle);
                    }
                    result = preresult;
                    result.Object = resultobject;
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }

    }
}
