using System;
using System.Collections.Generic;

namespace DL;

public partial class Carrito
{
    public int IdCarrito { get; set; }

    public string? IdUsuario { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual ICollection<DetalleCarrito> DetalleCarritos { get; set; } = new List<DetalleCarrito>();

    public virtual AspNetUser? IdUsuarioNavigation { get; set; }
}
