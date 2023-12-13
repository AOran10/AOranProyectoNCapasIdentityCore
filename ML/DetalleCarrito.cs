using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class DetalleCarrito
    {
        public int? IdDetalleCarrito { get; set; }
        public ML.Carrito? Carrito { get; set; }
        public ML.Producto? Producto { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Subtotal { get; set; }
        public List<object>? DetalleCarritos { get; set; }
    }
}
