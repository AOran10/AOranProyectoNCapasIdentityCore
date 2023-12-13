using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PL_Identity.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProductoController : Controller
    {
        

        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Producto.GetAll();
            ML.Producto producto = new ML.Producto();
            producto.Productos = result.Objects.ToList();

            return View(producto);
        }
        [HttpGet]
        public IActionResult Form(int? Id)
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
                ML.Result result = BL.Producto.GetById(Id.Value);
                producto = (ML.Producto)result.Object;
                ML.Result resultDepartamentos = BL.Departamento.GetByArea(producto.Departamento.Area.IdArea.Value);
                producto.Departamento.Departamentos = resultDepartamentos.Objects;
                }
            ML.Result resultAreas = BL.Area.GetAll();
            producto.Departamento.Area.Areas = resultAreas.Objects;
            return View(producto);
        }
        [HttpPost]
        public IActionResult Form(ML.Producto producto, IFormFile fuImagen)
            {
            if ((producto.Imagen != null && fuImagen != null) || (producto.Imagen == null && fuImagen != null))
            {
                producto.Imagen = ConvertirImagenABytes(fuImagen);
            }


            if (producto.Id == 0)
            {
                ViewBag.Subtitle = "Ingresar producto nuevo";
                ML.Result result = BL.Producto.Add(producto);
                if (result.Correct)
                {
                    ViewBag.Message = result.ErrorMessage;
                }
                else
                {
                    ViewBag.Message = "Error: " +result.ErrorMessage;
                }
                return View("Modal");
                ViewBag.Accion = "Ingresar";
                
            }
            else
            {
                ViewBag.Subtitle = "Actualizar producto";
                ML.Result result = BL.Producto.Update(producto);
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
        public IActionResult Delete(int id)
        {
            ViewBag.Subtitle = "Eliminar producto";
            ML.Result result = BL.Producto.Delete(id);
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
    }
}
