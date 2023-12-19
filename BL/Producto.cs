using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BL
{
    
    public class Producto
    {
        public static ML.Result GetAll(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var query = context.ProductoViews
						.FromSqlRaw("EXECUTE ProductoGetAll @IdDepartamento, @IdArea, @Nombre",
				new SqlParameter("@IdDepartamento", producto.Departamento.IdDepartamento),
				new SqlParameter("@IdArea", producto.Departamento.Area.IdArea),
				new SqlParameter("@Nombre", producto.Nombre))
			        .ToList();

					if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.Producto productoConsulta = new ML.Producto();

                            productoConsulta.Id = item.IdProducto;
                            productoConsulta.Nombre = item.NombreProducto;
                            productoConsulta.Descripcion = item.Descripcion;
                            productoConsulta.Stock = (int)item.Stock;
                            productoConsulta.EnCurso = (int)item.EnCurso;
                            productoConsulta.Imagen = item.Imagen;
                            productoConsulta.Precio = item.Precio;
                            productoConsulta.Departamento = new ML.Departamento();
                            productoConsulta.Departamento.IdDepartamento = (int)item.IdDepartamento;
                            productoConsulta.Departamento.Nombre = item.NombreDepartamento;
                            productoConsulta.Departamento.Area = new ML.Area();
                            productoConsulta.Departamento.Area.IdArea = (int)item.IdArea;
                            productoConsulta.Departamento.Area.Nombre = item.NombreArea;

                            result.Objects.Add(productoConsulta);
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
        public static ML.Result GetById(int IdProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var query = (from producto in context.Productos
                                 join departamento in context.Departamentos on producto.IdDepartamento equals departamento.IdDepartamento
                                 join area in context.Areas on departamento.IdArea equals area.IdArea
                                 where producto.IdProducto == IdProducto
                                 select new
                                 {
                                     IdProducto = producto.IdProducto,
                                     Nombre = producto.Nombre,
                                     Descripcion = producto.Descripcion,
                                     Stock = producto.Stock,
                                     EnCurso = producto.EnCurso,
                                     Imagen = producto.Imagen,
                                     Precio = producto.Precio,
                                     IdDepartamento = producto.IdDepartamento,
                                     NombreDepartamento = departamento.Nombre,
                                     IdArea = departamento.IdArea,
                                     NombreArea = area.Nombre
                                 }
                                 ).ToList();
                    if (query != null)
                    {
                        var item = query.FirstOrDefault();
                        ML.Producto producto = new ML.Producto();
                            producto.Id = item.IdProducto;
                            producto.Nombre = item.Nombre;
                            producto.Descripcion = item.Descripcion;
                            producto.Stock = (int)item.Stock;
                            producto.EnCurso = item.EnCurso.Value;
                            producto.Imagen = item.Imagen;
                            producto.Precio = item.Precio;
                            producto.Departamento = new ML.Departamento();
                            producto.Departamento.IdDepartamento = (int)item.IdDepartamento;
                            producto.Departamento.Nombre = item.NombreDepartamento;
                            producto.Departamento.Area = new ML.Area();
                            producto.Departamento.Area.IdArea = (int)item.IdArea;
                            producto.Departamento.Area.Nombre = item.NombreArea;

                        result.Object = producto;
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
        public static ML.Result Add(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    DL.Producto productoLINQ = new DL.Producto();
                        productoLINQ.Nombre = producto.Nombre;
                        productoLINQ.Descripcion = producto.Descripcion;
                        productoLINQ.Stock = producto.Stock;
                        productoLINQ.Precio = producto.Precio;
                        productoLINQ.EnCurso = producto.EnCurso != null ? producto.EnCurso : 0;
                        productoLINQ.IdDepartamento = producto.Departamento.IdDepartamento;

                    productoLINQ.Imagen = producto.Imagen;
                        context.Productos.Add(productoLINQ);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected >0)
                    {
                        result.Correct = true;
                        result.ErrorMessage = "Se ha ingresado el producto correctamente";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se ingreso el producto";
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
        public static ML.Result Update(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var productoLINQ = (from productoconsulta in context.Productos
                                 where productoconsulta.IdProducto == producto.Id
                                 select productoconsulta).SingleOrDefault();
                    if (productoLINQ != null)
                    {
                        productoLINQ.Nombre = producto.Nombre;
                        productoLINQ.Descripcion = producto.Descripcion;
                        productoLINQ.Stock = producto.Stock;
                        productoLINQ.EnCurso = producto.EnCurso;
                        productoLINQ.Imagen = producto.Imagen;
                        productoLINQ.Precio = producto.Precio;
                        productoLINQ.IdDepartamento = producto.Departamento.IdDepartamento;
                        context.Productos.Update(productoLINQ);
                        int rowsAffected = context.SaveChanges();

                        if (rowsAffected >0)
                        {
                            result.Correct = true;
                            result.ErrorMessage = "Producto Actualizado";
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo actualizar el producto";
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
        public static ML.Result Delete(int IdProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var productoLINQ = (from productoconsulta in context.Productos
                                        where productoconsulta.IdProducto == IdProducto
                                        select productoconsulta).SingleOrDefault();
                    if (productoLINQ != null)
                    {
                        
                        context.Productos.Remove(productoLINQ);
                        int rowsAffected = context.SaveChanges();

                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                            result.ErrorMessage = "Producto Eliminado";
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo eliminar el producto";
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
