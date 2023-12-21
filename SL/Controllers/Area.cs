using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    public class Area : Controller
    {
        [HttpPost("getall")]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Area.GetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
        [HttpGet("getbyid/{idArea}")]
        public IActionResult GetById(int idArea)
        {
            ML.Result result = BL.Area.GetById(idArea);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
        [HttpPost("add")]
        public IActionResult Add(ML.Area area)
        {
            ML.Result result = BL.Area.Add(area);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
        [HttpPut("update")]
        public IActionResult Update(ML.Area area)
        {
            ML.Result result = BL.Area.Update(area);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int idArea)
        {
            ML.Result result = BL.Area.Delete(idArea);
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
