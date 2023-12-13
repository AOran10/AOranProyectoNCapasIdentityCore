using Microsoft.AspNetCore.Mvc;

namespace PL_Identity.Controllers
{
    public class AreaController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Area.GetAll();
            ML.Area area = new ML.Area();
            area.Areas = result.Objects;
            return View(area);
        }
        [HttpGet]
        public IActionResult Form(int? IdArea)
        {
            ML.Area area = new ML.Area();
            if (IdArea == null)
            {
                ViewBag.Accion = "Ingresar";
                ViewBag.Subtitle = "Ingresar area nueva";
            }
            else
            {
                ViewBag.Accion = "Actualizar";
                ViewBag.Subtitle = "Actualizar area";
                ML.Result result = BL.Area.GetById(IdArea.Value);
                area = (ML.Area)result.Object;
            }
            return View(area);
        }
        [HttpPost]
        public IActionResult Form(ML.Area area)
        {
            if (area.IdArea == 0 || area.IdArea == null)
            {
                ViewBag.Subtitle = "Ingresar area nueva";
                ViewBag.Accion = "Ingresar";
                ML.Result result = BL.Area.Add(area);
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
            else
            {
                ViewBag.Subtitle = "Actualizar area";
                ViewBag.Accion = "Actializar";
                ML.Result result = BL.Area.Update(area);
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

        }
        public IActionResult Delete(int IdArea)
        {
            ViewBag.Subtitle = "Eliminar area";
            ML.Result result = BL.Area.Delete(IdArea);
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
    }
}
