using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Area
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var query = (from area in context.Areas
                                 select new
                                 {
                                     IdArea = area.IdArea,
                                     Nombre = area.Nombre,
                                     Descripcion = area.Descripcion
                                 }
                                 ).ToList();
                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.Area area = new ML.Area();
                            area.IdArea = item.IdArea;
                            area.Nombre = item.Nombre;
                            area.Descripcion = item.Descripcion;

                            result.Objects.Add(area);
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
        public static ML.Result GetById(int IdArea)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var query = (from areaConsulta in context.Areas
                                 where areaConsulta.IdArea == IdArea
                                 select new
                                 {
                                     IdArea = areaConsulta.IdArea,
                                     Nombre = areaConsulta.Nombre,
                                     Descripcion = areaConsulta.Descripcion
                                 }
                                 ).SingleOrDefault();
                    if (query != null)
                    {
                        var item = query;
                        ML.Area area = new ML.Area();
                        area.IdArea = item.IdArea;
                        area.Nombre = item.Nombre;
                        area.Descripcion = item.Descripcion;

                        result.Object = area;
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
        public static ML.Result Add(ML.Area area)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    DL.Area areaLINQ = new DL.Area();
                    areaLINQ.Nombre = area.Nombre;
                    areaLINQ.Descripcion = area.Descripcion;

                    context.Areas.Add(areaLINQ);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                        result.ErrorMessage = "Se ha ingresado el area correctamente";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se ingreso el area";
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
        public static ML.Result Update(ML.Area area)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var areaLINQ = (from areaConsult in context.Areas
                                    where areaConsult.IdArea == area.IdArea
                                    select areaConsult).SingleOrDefault();
                    if (areaLINQ != null)
                    {
                        areaLINQ.Nombre = area.Nombre;
                        areaLINQ.Descripcion = area.Descripcion;


                        context.Areas.Update(areaLINQ);

                        int rowsAffected = context.SaveChanges();

                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                            result.ErrorMessage = "Area actualizada";
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo actualizar el area";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo actualizar el area";
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
        public static ML.Result Delete(int IdArea)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var areaLINQ = (from areaConsult in context.Areas
                                    where areaConsult.IdArea == IdArea
                                    select areaConsult).SingleOrDefault();
                    if (areaLINQ != null)
                    {

                        context.Areas.Remove(areaLINQ);
                        int rowsAffected = context.SaveChanges();

                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                            result.ErrorMessage = "Area eliminada";
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo eliminar el area";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo eliminar el area";
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
