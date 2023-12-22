using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;

namespace PL_Identity.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProductoController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            ML.Producto producto = new ML.Producto();

            producto.Nombre = "";
            producto.Departamento = new ML.Departamento();
            producto.Departamento.IdDepartamento = 0;
            producto.Departamento.Area = new ML.Area();
            producto.Departamento.Area.IdArea = 0;

            //ML.Result result = BL.Producto.GetAll(producto);
            var result = await ProductoGetAll(producto);

            producto.Productos = result.Objects.ToList();

            return View(producto);
        }
        [HttpGet]
        public async Task<IActionResult>  Form(int? Id)
        {
            ML.Producto producto = new ML.Producto();
            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();
            if (Id == null)
            {
                ViewBag.Accion = "Ingresar";
                ViewBag.Subtitle = "Ingresar producto nuevo";
            }
            else
            {
                ViewBag.Accion = "Actualizar";
                ViewBag.Subtitle = "Actualizar producto";
                var result = await ProductoGetById(Id.Value);
                producto = result;
                ML.Result resultDepartamentos = BL.Departamento.GetByArea(producto.Departamento.Area.IdArea.Value);
                producto.Departamento.Departamentos = resultDepartamentos.Objects;
                }
            ML.Result resultAreas = BL.Area.GetAll();
            producto.Departamento.Area.Areas = resultAreas.Objects;
            return View(producto);
        }
        [HttpPost]
        public async Task<IActionResult> Form(ML.Producto producto, IFormFile fuImagen)
            {
            if ((producto.Imagen != null && fuImagen != null) || (producto.Imagen == null && fuImagen != null))
            {
                producto.Imagen = ConvertirImagenABytes(fuImagen);
            }


            if (producto.Id == 0)
            {
                ViewBag.Subtitle = "Ingresar producto nuevo";
                ML.Result result = await ProductoAdd(producto);
                if (result.Correct)
                {
                    ViewBag.Message = result.ErrorMessage;
                }
                else
                {
                    ViewBag.Message = "Error: " +result.ErrorMessage;
                }
                return View("Modal");
                //ViewBag.Accion = "Ingresar";
                
            }
            else
            {
                ViewBag.Subtitle = "Actualizar producto";
                ML.Result result = await ProductoUpdate(producto);
                if (result.Correct)
                {
                    ViewBag.Message = result.ErrorMessage;
                    
                }
                else
                {
                    ViewBag.Message = "Error: " + result.ErrorMessage;
                    //result = BL.Producto.GetById(producto.Id);
                    //producto = (ML.Producto)result.Object;
                }
                return View("Modal");
                ViewBag.Accion = "Actializar";
            }


            return View(producto);
        }
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Subtitle = "Eliminar producto";
            ML.Result result = await ProductoDelete(id);
            if (result.Correct)
            {
                ViewBag.Message = result.ErrorMessage;
            }
            else
            {
                ViewBag.Message = "Error: " + result.ErrorMessage;
            }
            return View("Modal");
        }
        public byte[] ConvertirImagenABytes(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public JsonResult GetDepartamento(int IdArea)
        {
            var result = BL.Departamento.GetByArea(IdArea);
            return Json(result.Objects);
        }

        private async Task<ML.Producto> ProductoGetById(int Id)
        {
            ML.Producto producto = new ML.Producto();
            try
            {
                var options = new RestClientOptions("http://localhost:5286/api/Producto/getbyid/" + Id);
                var client = new RestClient(options);
                var request = new RestRequest("");
                var response = await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ML.Result preresult = System.Text.Json.JsonSerializer.Deserialize<ML.Result>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    string objparticular = preresult.Object.ToString();
                    ML.Producto resultobject = System.Text.Json.JsonSerializer.Deserialize<ML.Producto>(objparticular, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    producto = resultobject;
                }

            }
            catch (Exception ex)
            {

            }

            return producto;
        }
        private async Task<ML.Result> ProductoGetAll(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            Models.Consulta consulta = new Models.Consulta();
            consulta.ConsultaAbierta = producto.Nombre;
            consulta.IdArea = producto.Departamento.Area.IdArea;
            consulta.IdDepartamento = producto.Departamento.IdDepartamento;

            string consult = JsonConvert.SerializeObject(consulta);
            try
            {
                var options = new RestClientOptions("http://localhost:5286/api/Producto/getall/");
                var client = new RestClient(options);
                var request = new RestRequest("");

                request.AddJsonBody(consult, false);
                //var responseAuthentication = await clientAuthentication.PostAsync(requestAuthentication);
                var response = await client.PostAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ML.Result preresult = System.Text.Json.JsonSerializer.Deserialize<ML.Result>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    //string objparticular = preresult.Objects.ToString();
                    //ML.Producto resultobject = System.Text.Json.JsonSerializer.Deserialize<ML.Producto>(objparticular, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


                    if (preresult.Correct)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in preresult.Objects)
                        {
                            string objparticular = item.ToString();
                            ML.Producto productoconsulta = System.Text.Json.JsonSerializer.Deserialize<ML.Producto>(objparticular, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            result.Objects.Add(productoconsulta);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }
        private async Task<ML.Result> ProductoAdd(ML.Producto producto)
        {
            ML.Result result = new ML.Result();


            string consult = JsonConvert.SerializeObject(producto);
            try
            {
                var options = new RestClientOptions("http://localhost:5286/api/Producto/add/");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddJsonBody(consult, false);
                var response = await client.PostAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ML.Result preresult = System.Text.Json.JsonSerializer.Deserialize<ML.Result>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    result = preresult;
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }
        private async Task<ML.Result> ProductoUpdate(ML.Producto producto)
        {
            ML.Result result = new ML.Result();


            string consult = JsonConvert.SerializeObject(producto);
            try
            {
                var options = new RestClientOptions("http://localhost:5286/api/Producto/update/");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddJsonBody(consult, false);
                var response = await client.PutAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ML.Result preresult = System.Text.Json.JsonSerializer.Deserialize<ML.Result>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    result = preresult;
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }
        private async Task<ML.Result> ProductoDelete(int id)
        {
            ML.Result result = new ML.Result();


            try
            {
                var options = new RestClientOptions("http://localhost:5286/api/Producto/delete/" + id);
                var client = new RestClient(options);
                var request = new RestRequest("");
                var response = await client.DeleteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ML.Result preresult = System.Text.Json.JsonSerializer.Deserialize<ML.Result>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    result = preresult;
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}
