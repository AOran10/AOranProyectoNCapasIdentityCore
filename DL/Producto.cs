using System;
using System.Collections.Generic;

namespace DL;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Stock { get; set; }

    public int? EnCurso { get; set; }

    public byte[]? Imagen { get; set; }

    public int? IdDepartamento { get; set; }

    public decimal? Precio { get; set; }

    public virtual ICollection<DetalleCarrito> DetalleCarritos { get; set; } = new List<DetalleCarrito>();

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
