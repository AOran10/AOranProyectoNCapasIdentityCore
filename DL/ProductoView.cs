using System;
using System.Collections.Generic;

namespace DL;

public partial class ProductoView
{
    public int IdProducto { get; set; }

    public string? NombreProducto { get; set; }

    public string? Descripcion { get; set; }

    public int? Stock { get; set; }

    public int? EnCurso { get; set; }

    public byte[]? Imagen { get; set; }

    public decimal? Precio { get; set; }

    public int? IdDepartamento { get; set; }

    public string NombreDepartamento { get; set; } = null!;

    public int? IdArea { get; set; }

    public string NombreArea { get; set; } = null!;
}
