using System;
using System.Collections.Generic;

namespace DL;

public partial class RolesAsignado
{
    public int IdRolAsignado { get; set; }

    public string? IdRol { get; set; }

    public string? IdUsuario { get; set; }

    public virtual AspNetRole? IdRolNavigation { get; set; }

    public virtual AspNetUser? IdUsuarioNavigation { get; set; }
}
