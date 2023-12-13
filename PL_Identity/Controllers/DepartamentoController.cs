using Microsoft.AspNetCore.Mvc;

namespace PL_Identity.Controllers
{
    public class DepartamentoController : Controller
    {
        
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Departamento departamento = new ML.Departamento();
            departamento.Area = new ML.Area();
            ML.Result resultAreas = BL.Area.GetAll();
            departamento.Area.Areas = resultAreas.Objects;

            return View(departamento);
        }
        [HttpPost]
        public IActionResult GetAll(ML.Departamento departamento)
        {
            
            if (departamento.Area.IdArea != null || departamento.Area.IdArea != 0)
            {
                ML.Result result = BL.Departamento.GetByArea(departamento.Area.IdArea.Value);
                departamento.Departamentos = result.Objects;
            }
            else
            {
                departamento.Departamentos = new List<object>();
            }
            ML.Result resultAreas = BL.Area.GetAll();
            departamento.Area.Areas = resultAreas.Objects;

            return View(departamento);
        }
        [HttpGet]
        public IActionResult Form(int? IdDepartamento, int IdArea)
        {
            ML.Departamento departamento = new ML.Departamento();
            departamento.Area = new ML.Area();
            if (IdDepartamento == null)
            {
                ViewBag.Accion = "Ingresar";
                ViewBag.Subtitle = "Ingresar departamento nuevo";
            }
            else
            {
                ViewBag.Accion = "Actualizar";
                ViewBag.Subtitle = "Actualizar departamento";
                ML.Result result = BL.Departamento  .GetById(IdDepartamento.Value);
                departamento = (ML.Departamento)result.Object;
            }
            ML.Result resultArea = BL.Area.GetById(IdArea);
            ML.Area area = (ML.Area)resultArea.Object;
            departamento.Area = area;
            return View(departamento);
        }
        [HttpPost]
        public IActionResult Form(ML.Departamento departamento, int IdArea)
        {
            departamento.Area = new ML.Area();
            departamento.Area.IdArea = IdArea;
            if (departamento.IdDepartamento == 0 || departamento.IdDepartamento == null)
            {
                ViewBag.Subtitle = "Ingresar departamento nuevo";
                ViewBag.Accion = "Ingresar";
                ML.Result result = BL.Departamento.Add(departamento);
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
                ViewBag.Subtitle = "Actualizar departamento";
                ViewBag.Accion = "Actualizar";
                ML.Result result = BL.Departamento.Update(departamento);
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
        public IActionResult Delete(int IdDepartamento)
        {
            ViewBag.Subtitle = "Eliminar departamento";
            ML.Result result = BL.Departamento.Delete(IdDepartamento);
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
