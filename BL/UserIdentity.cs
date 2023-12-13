using Microsoft.EntityFrameworkCore;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class UserIdentity
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var query = (from user in context.AspNetUsers
                                 select new
                                 {
                                     IdUser = user.Id,
                                     UserName = user.UserName
                                 }).ToList();



                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.UserIdentity usuario = new ML.UserIdentity();
                            usuario.IdUsuario = item.IdUser;
                            usuario.UserName = item.UserName;
                            result.Objects.Add(usuario);
                        }

                        result.Correct = true;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.InnerException.Message;
                result.Ex = ex;
            }

            return result;
        }
        public static ML.Result Asignar(ML.UserIdentity user)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"AddAspNetUserRoles '{user.IdUsuario}', '{user.Rol.RoleId}'");

                    if (query != null)
                    {
                        result.Correct = true;
                        result.ErrorMessage = "Se ha asignado un rol al usuario";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo asignar al hacer la asignacion";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.InnerException.Message;
                result.Ex = ex;
            }

            return result;

        }
        public static string GetByName(string UserName) 
        {
            string IdUser;
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var query = (from user in context.AspNetUsers
                                 where user.UserName == UserName
                                 select new
                                 {
                                     IdUser = user.Id,
                                     UserName = user.UserName
                                 }).FirstOrDefault();



                    if (query != null)
                    {
                        var item = query;
                        IdUser = item.IdUser;

                    }
                    else
                    {
                         IdUser = "";
                    }
                }

            }
            catch (Exception ex)
            {
                IdUser = "";
            }
            return IdUser;
        }
    }
}
