using Microsoft.AspNetCore.Mvc;
using ML;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Producto : ControllerBase
    {
        // GET: api/<Producto>
        [HttpPost("getall")]
        public IActionResult GetAll(ML.Consulta consulta)
        {
            ML.Producto producto = new ML.Producto();
            producto.Nombre = consulta.ConsultaAbierta;
            producto.Departamento = new ML.Departamento();
            producto.Departamento.IdDepartamento = consulta.IdDepartamento != null ? consulta.IdDepartamento.Value : 0;
            producto.Departamento.Area = new ML.Area();
            producto.Departamento.Area.IdArea = consulta.IdArea != null ? consulta.IdArea.Value : 0;
            ML.Result result = BL.Producto.GetAll(producto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        // GET api/<Producto>/5
        [HttpGet("getbyid/{idProducto}")]
        public IActionResult GetById(int idProducto)   
        {
            ML.Result result = BL.Producto.GetById(idProducto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        // POST api/<Producto>
        [HttpPost("add")]
        public IActionResult Add(ML.Producto producto)
        {
            ML.Result result = BL.Producto.Add(producto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        // PUT api/<Producto>/5
        [HttpPut("update")]
        public IActionResult Update(ML.Producto producto)
        {
            ML.Result result = BL.Producto.Update(producto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        // DELETE api/<Producto>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int idProducto)
        {
            ML.Result result = BL.Producto.Delete(idProducto);  
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}
