using DL;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace BL
{
    public class Carrito
    {
        public static ML.Result GetCarrito(string UserName)
        {
            ML.Result result = new ML.Result();
            try
            {
                string IdUsuario = BL.UserIdentity.GetByName(UserName);
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryCarrito = (from carritoLINQ in context.Carritos
                                 where carritoLINQ.IdUsuario == IdUsuario
                                        select new
                                 {
                                     IdCarrito = carritoLINQ.IdCarrito,
                                     IdUsuario = carritoLINQ.IdUsuario
                                 }
                                 ).FirstOrDefault();
                    if (queryCarrito != null)
                    {
                        var item = queryCarrito;
                        ML.Carrito carrito = new ML.Carrito();
                        carrito.IdCarrito = item.IdCarrito;
                        carrito.Usuario = new ML.UserIdentity();
                        carrito.Usuario.IdUsuario = item.IdUsuario;

                        var queryDetalleCarrito = (from carritoLINQ in context.Carritos
                                                    join detalleCarrito in context.DetalleCarritos on carritoLINQ.IdCarrito equals detalleCarrito.IdCarrito
                                                    join producto in context.Productos on detalleCarrito.IdProducto equals producto.IdProducto
                                                   where carritoLINQ.IdUsuario == carrito.Usuario.IdUsuario
                                                    select new
                                                    {
                                                        IdCarrito = carritoLINQ.IdCarrito,
                                                        IdDetalle = detalleCarrito.IdDetalleCarrito,
                                                        IdProducto = detalleCarrito.IdProducto,
                                                        NombreProducto = producto.Nombre,
                                                        Precio = producto.Precio,
                                                        Imagen = producto.Imagen,
                                                        Stock = producto.Stock,
                                                        Cantidad = detalleCarrito.Cantidad,
                                                        Subtotal = detalleCarrito.SubTotal,

                                                    }).ToList();
                        if (queryDetalleCarrito != null)
                        {
                            carrito.Carritos = new List<object>();
                            foreach (var itemDetalle in queryDetalleCarrito)
                            { 
                                ML.DetalleCarrito detalleCarrito = new ML.DetalleCarrito();
                                detalleCarrito.IdDetalleCarrito = itemDetalle.IdDetalle;
                                detalleCarrito.Producto = new ML.Producto();
                                detalleCarrito.Producto.Id = itemDetalle.IdProducto.Value;
                                detalleCarrito.Producto.Nombre = itemDetalle.NombreProducto;
                                detalleCarrito.Producto.Precio = itemDetalle.Precio.Value;
                                detalleCarrito.Producto.Imagen = itemDetalle.Imagen;
                                detalleCarrito.Producto.Stock = itemDetalle.Stock;
                                detalleCarrito.Cantidad = itemDetalle.Cantidad;
                                detalleCarrito.Subtotal = itemDetalle.Subtotal;


                                carrito.Carritos.Add(detalleCarrito);
                            }
                                result.ErrorMessage = "El Carrito recupero productos";
                        }
                        else
                        {
                            result.ErrorMessage = "La carrito está vacío";
                        }

                         result.Object = carrito;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = true;
                        result.ErrorMessage = "La carrito está vacío";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result AddToCar(ML.Orden orden)
        {
            ML.Result result = new ML.Result();
            try
            {
                orden.Usuario.IdUsuario = BL.UserIdentity.GetByName(orden.Usuario.UserName);
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryCarrito = (from carritoLINQ in context.Carritos
                                        where carritoLINQ.IdUsuario == orden.Usuario.IdUsuario
                                        select new
                                        {
                                            IdCarrito = carritoLINQ.IdCarrito,
                                            IdUsuario = carritoLINQ.IdUsuario

                                        }
                                 ).FirstOrDefault();
                    if (queryCarrito != null)
                    {
                        DL.DetalleCarrito productoNuevo = new DL.DetalleCarrito();
                        productoNuevo.IdCarrito = queryCarrito.IdCarrito;
                        productoNuevo.Cantidad = orden.Cantidad;
                        productoNuevo.IdProducto = orden.Producto.Id;
                        productoNuevo.SubTotal = orden.Producto.Precio * orden.Cantidad;

                        context.DetalleCarritos.Add(productoNuevo);
                        int rowsAffected = context.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                            result.ErrorMessage = "Se añadió un articulo nuevo satisfactoriamente";
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo añadir el producto";
                        }
                    }
                    else
                    {
                        DL.Carrito carrito = new DL.Carrito();
                        carrito.IdUsuario = orden.Usuario.IdUsuario;
                        carrito.Fecha = DateTime.Now;

                        context.Carritos.Add(carrito);
                        int rowsAffectedCarrito = context.SaveChanges();
                        if (rowsAffectedCarrito > 0)
                        {
                            result.ErrorMessage = "Se creó un carrito nuevo satisfactoriamente";

                            var queryCarritoCreado = (from carritoLINQ in context.Carritos
                                                where carritoLINQ.IdUsuario == orden.Usuario.IdUsuario
                                                select new
                                                {
                                                    IdCarrito = carritoLINQ.IdCarrito,
                                                    IdUsuario = carritoLINQ.IdUsuario

                                                }
                                ).FirstOrDefault();

                            DL.DetalleCarrito productoNuevo = new DL.DetalleCarrito();

                            productoNuevo.IdCarrito = queryCarritoCreado.IdCarrito;
                            productoNuevo.Cantidad = orden.Cantidad;
                            productoNuevo.IdProducto = orden.Producto.Id;
                            productoNuevo.SubTotal = orden.Producto.Precio * orden.Cantidad;

                            context.DetalleCarritos.Add(productoNuevo);
                            int rowsAffectedDetalleCarrito = context.SaveChanges();
                            if (rowsAffectedDetalleCarrito > 0)
                            {
                                result.Correct = true;
                                result.ErrorMessage = "Se añadió un articulo nuevo satisfactoriamente";
                            }
                            else
                            {
                                result.Correct = false;
                                result.ErrorMessage = "No se pudo añadir el producto";
                            }
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo crear un carrito nuevo";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
                throw;
            }
            return result;
        }
        public static ML.Result Delete(int IdDetalleCarrito)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var productoLINQ = (from productoconsulta in context.DetalleCarritos
                                        where productoconsulta.IdDetalleCarrito == IdDetalleCarrito
                                        select productoconsulta).SingleOrDefault();
                    if (productoLINQ != null)
                    {

                        context.DetalleCarritos.Remove(productoLINQ);
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
        public static ML.Result DeleteCar(int IdCarrito)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryCarrito = (from carritoLINQ in context.Carritos
                                        where carritoLINQ.IdCarrito == IdCarrito
                                        select carritoLINQ).SingleOrDefault();
                    if (queryCarrito != null)
                    {
                        context.Carritos.Remove(queryCarrito);
                        int rowsAffected = context.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                            result.ErrorMessage = "Carrito eliminado";
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo eliminar el carrito";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo recuperar el carrito a eliminar";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
