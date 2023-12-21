using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using ML;
using Newtonsoft.Json;
using PL_Identity.Models;
using RestSharp;
using System.Diagnostics;
using System.Text.Json;

namespace PL_Identity.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			ML.Producto producto = new ML.Producto();
			producto.Departamento = new ML.Departamento();
			producto.Departamento.Area = new ML.Area();

			ML.Result resultAreas = BL.Area.GetAll();
			producto.Departamento.Area.Areas = resultAreas.Objects;

			return View(producto);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
        public JsonResult GetProductosIndex(int? IdArea, int? IdDepartamento, string? Nombre)
         {
			ML.Producto producto = new ML.Producto();
	
			producto.Nombre = Nombre != null ? Nombre : "";
            producto.Departamento = new ML.Departamento();
			producto.Departamento.IdDepartamento = IdDepartamento != null ? IdDepartamento.Value : 0;

            producto.Departamento.Area = new ML.Area();
			producto.Departamento.Area.IdArea = IdArea != null ? IdArea.Value : 0;

            var result = BL.Producto.GetAll(producto);
            return Json(result.Objects);
        }
        public async Task<ActionResult> ProductoGet(int Id)
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

            return View(producto);
        }
        public async Task<JsonResult>  GetDepartamento(int idArea)
        {
            dynamic json = "";
            try
            {
                var options = new RestClientOptions("http://localhost:5286/api/Departamento/getbyarea/" + idArea);
                var client = new RestClient(options);
                var request = new RestRequest("");
                var response = await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ML.Result preresult = System.Text.Json.JsonSerializer.Deserialize<ML.Result>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    json = preresult;
                }

            }
            catch (Exception ex)
            {

            }

            return Json(json);

        }
    }
}