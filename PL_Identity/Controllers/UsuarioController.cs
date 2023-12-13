using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL_Identity.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Result result = BL.Usuario.GetAll();
            ML.Usuario usuario = new ML.Usuario();
            usuario.Usuarios = result.Objects.ToList();
            return View(usuario);
        }
    }
}
