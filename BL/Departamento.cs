using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Departamento
    {
        public static ML.Result GetByArea(int IdArea)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var query = (from departamento in context.Departamentos
                                 join area in context.Areas on departamento.IdArea equals area.IdArea
                                 where departamento.IdArea == IdArea
                                 select new
                                 {
                                     IdDepartamento = departamento.IdDepartamento,
                                     Nombre = departamento.Nombre,
                                     Descripcion = departamento.Descripcion,
                                     IdArea = departamento.IdArea,
                                     NombreArea = area.Nombre
                                 }
                                 ).ToList();
                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.Departamento departamento = new ML.Departamento();
                            departamento.IdDepartamento = item.IdDepartamento;
                            departamento.Nombre = item.Nombre;
                            departamento.Descripcion = item.Descripcion;
                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = item.IdArea;
                            departamento.Area.Nombre = item.NombreArea;

                            result.Objects.Add(departamento);
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
        public static ML.Result GetById(int IdDepartamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var query = (from departamento in context.Departamentos
                                 join area in context.Areas on departamento.IdArea equals area.IdArea
                                 where departamento.IdDepartamento == IdDepartamento
                                 select new
                                 {
                                     IdDepartamento = departamento.IdDepartamento,
                                     Nombre = departamento.Nombre,
                                     Descripcion = departamento.Descripcion,
                                     IdArea = departamento.IdArea,
                                     NombreArea = area.Nombre
                                 }
                                 ).SingleOrDefault();
                    if (query != null)
                    {
                        var item = query;
                        ML.Departamento departamento = new ML.Departamento();

                        departamento.IdDepartamento = item.IdDepartamento;
                        departamento.Nombre = item.Nombre;
                        departamento.Descripcion = item.Descripcion;
                        departamento.Area = new ML.Area();
                        departamento.Area.IdArea = item.IdArea;
                        departamento.Area.Nombre = item.NombreArea;


                        result.Object = departamento;
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
        public static ML.Result Add(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    DL.Departamento departamentoLINQ = new DL.Departamento();
                    departamentoLINQ.Nombre = departamento.Nombre;
                    departamentoLINQ.Descripcion = departamento.Descripcion;
                    departamentoLINQ.IdArea = departamento.Area.IdArea;

                    context.Departamentos.Add(departamentoLINQ);

                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                        result.ErrorMessage = "Se ha ingresado el departamento correctamente";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se ingreso el departamento";
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
        public static ML.Result Update(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var departamentoLINQ = (from departamentoconsulta in context.Departamentos
                                        where departamentoconsulta.IdDepartamento == departamento.IdDepartamento
                                        select departamentoconsulta).SingleOrDefault();
                    if (departamentoLINQ != null)
                    {
                        departamentoLINQ.Nombre = departamento.Nombre;
                        departamentoLINQ.Descripcion = departamento.Descripcion;
                        departamentoLINQ.IdArea = departamento.Area.IdArea;

                        context.Departamentos.Update(departamentoLINQ);
                        int rowsAffected = context.SaveChanges();

                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                            result.ErrorMessage = "Departamento actualizado";
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo actualizar el departamento";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo actualizar el producto";
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
        public static ML.Result Delete(int IdDepartamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var departamentoLINQ = (from departamentoconsulta in context.Departamentos
                                            where departamentoconsulta.IdDepartamento == IdDepartamento
                                            select departamentoconsulta).SingleOrDefault();
                    if (departamentoLINQ != null)
                    {

                        context.Departamentos.Remove(departamentoLINQ);
                        int rowsAffected = context.SaveChanges();

                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                            result.ErrorMessage = "Departamento eliminado";
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo eliminar el departamento";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo eliminar el producto";
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
