using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Estatus
    {
        public static ML.Result GetAll() 
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryEstatus = (from estatusLINQ in context.Estatuses
                                       select new
                                       {
                                           IdEstatus = estatusLINQ.IdEstatus,
                                           Nombre = estatusLINQ.Estatus1
                                       }
                                 ).ToList();
                    if (queryEstatus != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in queryEstatus)
                        {
                            ML.Estatus estatus = new ML.Estatus();
                            estatus.IdEstatus = item.IdEstatus;
                            estatus.NombreEstatus = item.Nombre;

                            result.Objects.Add(estatus);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se recolectaron estatus";
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Correct = false;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result UpdateEstatus(int IdPedido, int IdEstatus)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AoranProyectoNcapasIdentityCoreContext context = new DL.AoranProyectoNcapasIdentityCoreContext())
                {
                    var queryPedido = (from pedidoLINQ in context.Pedidos
                                       where pedidoLINQ.IdPedido == IdPedido
                                       select pedidoLINQ).SingleOrDefault();
                    if (queryPedido != null)
                    {
                        queryPedido.IdEstatus = IdEstatus;

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
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo localizar el pedido";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
