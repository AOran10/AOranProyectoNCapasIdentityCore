using System;
using System.Collections.Generic;

namespace DL;

public partial class Departamento
{
    public int IdDepartamento { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int? IdArea { get; set; }

    public virtual Area? IdAreaNavigation { get; set; }
}
