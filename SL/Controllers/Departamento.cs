using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Departamento : ControllerBase
    {
        // GET api/<Departamento>/5
        [HttpGet("getbyarea/{idArea}")]
        public IActionResult GetById(int idArea)
        {
            ML.Result result = BL.Departamento.GetByArea(idArea);
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
