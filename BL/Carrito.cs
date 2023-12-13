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
        public static ML.Result GetCarrito(string IdUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
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
                                                        IdDetaleCarrito = detalleCarrito.IdDetalleCarrito,
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
        public static ML.Result AddToCar(ML.Orden)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {

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
    }
}
