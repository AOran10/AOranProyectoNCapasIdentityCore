using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var query = (from usuarios in context.AspNetUsers
                                 select new
                                 {
                                     Id = usuarios.Id,
                                     UserName = usuarios.UserName,
                                     Email = usuarios.Email,
                                    NormalizedEmail = usuarios.NormalizedEmail,
                                    PasswordHash = usuarios.PasswordHash,
                                    SecurityStamp = usuarios.SecurityStamp,
                                    ConcurrencyStamp = usuarios.ConcurrencyStamp,
                                    PhoneNumber = usuarios.PhoneNumber,
                                    PhoneNumberConfirmed = usuarios.PhoneNumberConfirmed,
                                    TwoFactorEnabled = usuarios.TwoFactorEnabled,
                                    LockoutEnd = usuarios.LockoutEnd,
                                    LockoutEnabled = usuarios.LockoutEnabled,
                                    AccessFailedCount = usuarios.AccessFailedCount
                                 }
                                 ).ToList();
                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach ( var item in query )
                        {
                            ML.Usuario usuario = new ML.Usuario();

                            usuario.Id = item.Id;
                            usuario.UserName = item.UserName;
                            usuario.Email = item.Email;
                            usuario.NormalizedEmail = item.NormalizedEmail;
                            usuario.PasswordHash = item.PasswordHash;
                            usuario.SecurityStamp = item.SecurityStamp;
                            usuario.ConcurrencyStamp = item.ConcurrencyStamp;
                            usuario.PhoneNumber = item.PhoneNumber;
                            usuario.PhoneNumberConfirmed = item.PhoneNumberConfirmed;
                            usuario.TwoFactorEnabled = item.TwoFactorEnabled;
                            usuario.LockoutEnd = item.LockoutEnd;
                            usuario.LockoutEnabled = item.LockoutEnabled;

                            result.Objects.Add(usuario);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "La consulta está vacía";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
                //throw;
            }
            return result;
        }
    }
}
