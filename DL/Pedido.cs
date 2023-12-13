using System;
using System.Collections.Generic;

namespace DL;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public string? IdUsuario { get; set; }

    public DateTime? Fecha { get; set; }

    public int? IdEstatus { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Estatus? IdEstatusNavigation { get; set; }

    public virtual AspNetUser? IdUsuarioNavigation { get; set; }
}
