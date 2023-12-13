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
			ML.Result result = BL.Producto.GetAll();
			ML.Producto producto = new ML.Producto();
			producto.Productos = result.Objects;
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
        public JsonResult GetProductosIndex()
        {
			var result = BL.Producto.GetAll();
            return Json(result.Objects);
        }
        public ActionResult ProductoGet(int Id)
        {
            ML.Result result = BL.Producto.GetById(Id);

            ML.Producto producto = (ML.Producto)result.Object;

            return View(producto);
        }
    }
}