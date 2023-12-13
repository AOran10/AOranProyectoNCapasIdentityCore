using System;
using System.Collections.Generic;

namespace DL;

public partial class Estatus
{
    public int IdEstatus { get; set; }

    public string? Estatus1 { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
