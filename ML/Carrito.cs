using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public ML.UserIdentity Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public List<object>? Carritos { get; set; }
    }
}
