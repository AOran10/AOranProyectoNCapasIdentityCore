using DL;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Pedido
    {
        public static ML.Result AddPedido(int IdCarrito)
        {
            ML.Result result = new ML.Result();
            string IdUsuarioGlobal;
            int IdCarritoGlobal = IdCarrito;
            int IdDetalleCarritoGlobal;
            int IdPedidoGlobal;
            int IdDetallePedidoGlobal;
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryCarrito = (from carritoLINQ in context.Carritos
                                        where carritoLINQ.IdCarrito == IdCarrito
                                        select new
                                        {
                                            IdCarrito = carritoLINQ.IdCarrito,
                                            IdUsuario = carritoLINQ.IdUsuario
                                        }
                                 ).SingleOrDefault();
                    if ( queryCarrito != null ) 
                    {
                        var itemCarrito = queryCarrito;
                        IdUsuarioGlobal = itemCarrito.IdUsuario;

                        DL.Pedido pedidoNuevo = new DL.Pedido();
                        pedidoNuevo.IdUsuario = itemCarrito.IdUsuario;
                        pedidoNuevo.Fecha = DateTime.Now;
                        pedidoNuevo.IdEstatus = 1;

                        context.Pedidos.Add(pedidoNuevo);
                        int rowsAffectedPedido = context.SaveChanges();
                        if (rowsAffectedPedido > 0)
                        {
                            var queryPedido = (from pedidoLINQ in context.Pedidos
                                               where pedidoLINQ.IdUsuario == IdUsuarioGlobal
                                               where pedidoLINQ.IdEstatus == 1
                                               select new
                                               {
                                                   IdPedido = pedidoLINQ.IdPedido,
                                                   IdUsuario = pedidoLINQ.IdUsuario
                                               }
                                               ).FirstOrDefault();
                            if ( queryPedido != null )
                            {
                                var itemPedido = queryPedido;
                                ML.Result resultAddPedido = BL.Pedido.AddDetallePedido(itemPedido.IdPedido, IdCarrito);
                                if (resultAddPedido.Correct)
                                {
                                    ML.Result resultVaciar = BL.Pedido.EmptyContentCar(IdCarrito);
                                    if (resultVaciar.Correct)
                                    {
                                        ML.Result resultDeleteCar = BL.Carrito.DeleteCar(IdCarrito);
                                        if (resultDeleteCar.Correct)
                                        {
                                            ML.Result resultTotal = BL.Pedido.SetTotal(itemPedido.IdPedido);
                                            if (resultTotal.Correct)
                                            {
												ML.Result resultProcesoUpdate = BL.Pedido.SetEnProcesoPedido(itemPedido.IdPedido);
												if (resultProcesoUpdate.Correct)
												{
													result.Correct = resultProcesoUpdate.Correct;
													result.ErrorMessage = resultProcesoUpdate.ErrorMessage;
												}
												else
												{
													result.Correct = resultProcesoUpdate.Correct;
													result.ErrorMessage = resultProcesoUpdate.ErrorMessage;
												}
											}
                                            else
                                            {
                                                result.Correct = resultTotal.Correct;
                                                result.ErrorMessage = resultTotal.ErrorMessage;
                                            }
                                        }
                                        else
                                        {
                                            result.Correct = resultDeleteCar.Correct;
                                            result.ErrorMessage = resultDeleteCar.ErrorMessage;
                                        }
                                    }
                                    else
                                    {
                                        result.Correct = resultVaciar.Correct;
                                        result.ErrorMessage = resultVaciar.ErrorMessage;
                                    }
                                }
                                else
                                {
                                    result.Correct = resultAddPedido.Correct;
                                    result.ErrorMessage = resultAddPedido.ErrorMessage;
                                }
                            }
                            else
                            {
                                result.ErrorMessage = "Hubo un error, no se pudo recuperar el pedido";
                                result.Correct = false;
                            }
                        }
                        else
                        {
                            result.ErrorMessage = "Hubo un error, no se pudo crear el pedido";
                            result.Correct = false;
                        }
                    }
                    else
                    {
                        result.ErrorMessage = "Hubo un error, no se pudo recuperar el carrito";
                        result.Correct = false;
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
        public static ML.Result AddDetallePedido(int IdPedido, int IdCarrito)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryDetalleCarrito = (from carritoLINQ in context.Carritos
                                               join detalleCarrito in context.DetalleCarritos on carritoLINQ.IdCarrito equals detalleCarrito.IdCarrito
                                               where carritoLINQ.IdCarrito == IdCarrito
                                               select new
                                               {
                                                   IdCarrito = carritoLINQ.IdCarrito,
                                                   IdDetalle = detalleCarrito.IdDetalleCarrito,
                                                   IdProducto = detalleCarrito.IdProducto,
                                                   Cantidad = detalleCarrito.Cantidad,
                                                   Subtotal = detalleCarrito.SubTotal
                                               }).ToList();
                    if (queryDetalleCarrito != null)
                    {
                        int productosExitosos = 0;
                        foreach (var item in queryDetalleCarrito)
                        {
                            DL.DetallePedido detalle = new DL.DetallePedido();
                            detalle.IdPedido = IdPedido;
                            detalle.IdProducto = item.IdProducto;
                            detalle.Cantidad = item.Cantidad;
                            detalle.SubTotal = item.Subtotal;

                            context.DetallePedidos.Add(detalle);
                            int rowsAffected = context.SaveChanges();
                            if (rowsAffected >0)
                            {
                                productosExitosos++;
                            }
                        }
                        if (productosExitosos > 0)
                        {
                            if (productosExitosos == queryDetalleCarrito.Count)
                            {
                                result.Correct = true;
                                result.ErrorMessage = "Se añadieron todos los productos al pedido";
                            }
                            else
                            {
                                result.Correct = false;
                                result.ErrorMessage = "Se han añadido parcialmente productos";
                            }
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudieron agregar los productos al pedido";
                        }
                    }
                    else
                    {
                        result.ErrorMessage = "Hubo un error, no se pudo recuperar el contenido del carrito";
                        result.Correct = false;
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
        public static ML.Result EmptyContentCar(int IdCarrito)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryDetalleCarrito = (from carritoLINQ in context.Carritos
                                               join detalleCarrito in context.DetalleCarritos on carritoLINQ.IdCarrito equals detalleCarrito.IdCarrito
                                               where carritoLINQ.IdCarrito == IdCarrito
                                               select new
                                               {
                                                   IdCarrito = carritoLINQ.IdCarrito,
                                                   IdDetalleCarrito = detalleCarrito.IdDetalleCarrito
                                               }).ToList();
                    if (queryDetalleCarrito != null)
                    {
                        int borradoExitosos = 0;
                        foreach (var item in queryDetalleCarrito)
                        {
                            ML.Result resultDelete = BL.Carrito.Delete(item.IdDetalleCarrito);
                            if (resultDelete.Correct)
                            {
                                borradoExitosos++;
                            }
                        }
                        if (borradoExitosos > 0)
                        {
                            if (borradoExitosos == queryDetalleCarrito.Count)
                            {
                                result.ErrorMessage = "Se ha vaciado el carrito correctamente";
                                result.Correct = true;
                            }
                            else
                            {
                                result.ErrorMessage = "Hubo un error al vaciar el carrito";
                                result.Correct = false;
                            }
                        }
                        else
                        {
                            result.ErrorMessage = "Hubo un error al vaciar el carrito";
                            result.Correct = false;
                        }
                    }
                    else
                    {
                        result.ErrorMessage = "Hubo un error, no se pudo recuperar el contenido del carrito para eliminar";
                        result.Correct = false;
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
        public static ML.Result SetEnProcesoPedido(int IdPedido)
            {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryPedido = (from pedidoLINQ in context.Pedidos
                                            where pedidoLINQ.IdPedido == IdPedido
                                            select pedidoLINQ).SingleOrDefault();
                    queryPedido.IdUsuario = queryPedido.IdUsuario;
                    queryPedido.Fecha = queryPedido.Fecha;
                    queryPedido.IdEstatus = 2;

                    context.Pedidos.Update(queryPedido);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                        result.ErrorMessage = "Estado del pedido actualizado";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo actualizar el estado del pedido";
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
        public static ML.Result SetTotal(int IdPedido)
        {
            ML.Result result = new ML.Result();
            try
            {
				using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
				{
					var queryTotal = (from subtotalesLINQ in context.DetallePedidos
									  where subtotalesLINQ.IdPedido == IdPedido
									  select new
									  {
										  Subtotal = subtotalesLINQ.SubTotal
									  }).ToList();

					if (queryTotal != null)
					{
                        var subtotales = queryTotal;
                        var total = subtotales.Sum(x => Convert.ToDecimal(x));

						var queryPedido = (from pedidoLINQ in context.Pedidos
										   where pedidoLINQ.IdPedido == IdPedido
										   select pedidoLINQ).FirstOrDefault();
                        if (queryPedido != null)
                        {
							queryPedido.Total = total;
                            context.Pedidos.Update(queryPedido);
							int rowsAffected = context.SaveChanges();

							if (rowsAffected > 0)
							{
								result.Correct = true;
								result.ErrorMessage = "Total Asignado";
							}
							else
							{
								result.Correct = false;
								result.ErrorMessage = "No se pudo asignar el total";
							}


						}
						else
						{
							result.Correct = false;
							result.ErrorMessage = "No se pudorecolectar el carrito";
						}
					}
                    else
                    {
						result.Correct = false;
                        result.ErrorMessage = "No se pudieron recolectar los subtotales";
					}
				}
			}
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage= ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result GetAllAdmin()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryPedido = (from pedidoLINQ in context.Pedidos
                                        select new
                                        {
                                            IdPedido = pedidoLINQ.IdPedido,
                                            IdUsuario = pedidoLINQ.IdUsuario,
                                            Fecha = pedidoLINQ.Fecha,
                                            IdEstatus = pedidoLINQ.IdEstatus
                                        }
                                 ).ToList();
                    if ( queryPedido != null )
                    {
                        result.Objects = new List<object>();
                        foreach (var item in queryPedido)
                        {
                            ML.Pedido pedido = new ML.Pedido();
                            pedido.IdPedido = item.IdPedido;
                            pedido.Usuario = new ML.UserIdentity();
                            pedido.Usuario.IdUsuario = item.IdUsuario;
                            pedido.Fecha = item.Fecha;
                            pedido.Estatus = new ML.Estatus();
                            pedido.Estatus.IdEstatus = item.IdEstatus.Value;

                            result.Objects.Add(pedido);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
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
        public static ML.Result GetPedidos(string UserName)
        {
            ML.Result result = new ML.Result();
            try
            {
                string IdUsuario = BL.UserIdentity.GetByName(UserName);
               
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryPedidos = (from pedidoLINQ in context.Pedidos
                                        join estatusLINQ in context.Estatuses on pedidoLINQ.IdEstatus equals estatusLINQ.IdEstatus
										where pedidoLINQ.IdUsuario == IdUsuario
                                       orderby pedidoLINQ.IdEstatus ascending
                                       select new
                                       {
                                           IdPedido = pedidoLINQ.IdPedido,
                                           IdDelUsuario = pedidoLINQ.IdUsuario,
                                           FechaPedido = pedidoLINQ.Fecha,
                                           IdEstatus = pedidoLINQ.IdEstatus,
                                           Total = pedidoLINQ.Total,
                                           NombreEstatus = estatusLINQ.Estatus1,
                                           Descripcion = estatusLINQ.Descripcion
                                       }
                                        ).ToList();
                    if (queryPedidos != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in queryPedidos)
                        {
                            ML.Pedido pedido = new ML.Pedido();
                            pedido.IdPedido = item.IdPedido;
                            pedido.Usuario = new ML.UserIdentity();
                            pedido.Usuario.IdUsuario = item.IdDelUsuario;
                            pedido.Fecha = item.FechaPedido;
                            pedido.Estatus = new ML.Estatus();
                            pedido.Estatus.IdEstatus = item.IdEstatus.Value;
                            pedido.Estatus.NombreEstatus = item.NombreEstatus;
                            pedido.Estatus.Descripcion = item.Descripcion;
                            pedido.Total = item.Total.Value;

                            ML.Result resultDetalle = BL.Pedido.GetDetallePedido(pedido.IdPedido);
                            pedido.DetallesPedidos = resultDetalle.Correct? result.Objects: new List<object>();

                            result.Objects.Add(pedido);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudieron recuperar los pedidos";
                    }
                }
            }
            catch (Exception ex )
            {
                result.Correct = true;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result GetDetallePedido(int idPedido)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryDetallePedido = (from detallePedidosLINQ in context.DetallePedidos
                                              join productoLINQ in context.Productos on detallePedidosLINQ.IdProducto equals productoLINQ.IdProducto
                                              where detallePedidosLINQ.IdPedido == idPedido
                                              select new
                                              {
                                                  IdDetallePedido = detallePedidosLINQ.IdDetallePedido,
                                                  IdProducto = detallePedidosLINQ.IdProducto,
                                                  Cantidad = detallePedidosLINQ.Cantidad,
                                                  SubTotal = detallePedidosLINQ.SubTotal,

                                                  Imagen = productoLINQ.Imagen,
                                                  Nombre = productoLINQ.Nombre,
                                              }).ToList();
                    if (queryDetallePedido != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var itemDetalle in queryDetallePedido)
                        {
                            ML.DetallePedido detallePedido = new ML.DetallePedido();
                            detallePedido.IdDetallePedido = itemDetalle.IdDetallePedido;

                            detallePedido.Producto = new ML.Producto();
                            detallePedido.Producto.Id = itemDetalle.IdProducto.Value;
                            detallePedido.Producto.Nombre = itemDetalle.Nombre;
                            detallePedido.Producto.Imagen = itemDetalle.Imagen;

                            detallePedido.Cantidad = itemDetalle.Cantidad;
                            detallePedido.Subtotal = itemDetalle.SubTotal;

                            result.Objects.Add(detallePedido);
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudieron recuperar los datos del pedido";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = true;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
