using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Carrito : ControllerBase
    {
        // GET: api/<Carrito>
        [HttpGet("get/{UserName}")]
        public IActionResult GetAll(string UserName)
        {
            ML.Result result = BL.Carrito.GetCarrito(UserName);
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
