using Microsoft.AspNetCore.Mvc;
using PL_Identity.Models;
using System.Diagnostics;

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
        public ActionResult ProductoGet(int Id)
        {
            ML.Result result = BL.Producto.GetById(Id);

            ML.Producto producto = (ML.Producto)result.Object;

            return View(producto);
        }
        public JsonResult GetDepartamento(int IdArea)
        {
            var result = BL.Departamento.GetByArea(IdArea);
            return Json(result.Objects);
        }
    }
}