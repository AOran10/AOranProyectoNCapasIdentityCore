using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PL_Identity.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RolController : Controller
    {

        private RoleManager<IdentityRole> roleManager;
        public RolController(RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var Roles = roleManager.Roles.ToList();
            return View(Roles);
        }
        [HttpGet]
        public IActionResult Asignar(Guid IdRole, string Name)  
        {
            ML.Result result = BL.UserIdentity.GetAll();
            ML.UserIdentity user = new ML.UserIdentity();
            if (result.Correct)
            {
                user.IdentityUsers = result.Objects;
            }
            user.Rol = new ML.Rol();
            user.Rol.Name = Name;
            user.Rol.RoleId = IdRole;

            return View(user);
        }
        [HttpPost]
        public IActionResult Asignar(ML.UserIdentity user)
        {
            ViewBag.Subtitle = "Asignar Rol";
            ML.Result result = BL.UserIdentity.Asignar(user);
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

        [HttpGet]
        public IActionResult Form(Guid? IdRole, string Name)
        {
            
            IdentityRole role = new IdentityRole();
            if (Name != null)
            {
                role.Name = Name;
                role.Id = IdRole.ToString();
                return View(role);
            }
            else
            {
                return View(role);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Form([Required] Microsoft.AspNetCore.Identity.IdentityRole rol)
        {

            if (ModelState.IsValid)
            {
                IdentityRole role = await roleManager.FindByIdAsync(rol.Id.ToString());
                //Add o Insert
                if (role == null)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(rol.Name));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetAll");
                    }
                    else
                    {
                        ViewBag.Subtitle = "Ingresar rol nuevo";
                        ViewBag.Message = "No se ha podido ingresar el rol";
                        return View("Modal");

                    }
                }
                else //Update
                {
                    role.Id = await roleManager.GetRoleIdAsync(rol);
                    role.Name = await roleManager.GetRoleNameAsync(rol);

                    IdentityResult result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetAll");
                    }
                    else
                    {
                        ViewBag.Subtitle = "Actualizar rol";
                        ViewBag.Message = "No se ha podido actualizar el rol";
                        return View("Modal");
                    }
                }
            }
            return View(rol);
        }
    }
}
