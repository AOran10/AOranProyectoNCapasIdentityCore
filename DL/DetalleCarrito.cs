using System;
using System.Collections.Generic;

namespace DL;

public partial class DetalleCarrito
{
    public int IdDetalleCarrito { get; set; }

    public int? IdCarrito { get; set; }

    public int? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? SubTotal { get; set; }

    public virtual Carrito? IdCarritoNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
