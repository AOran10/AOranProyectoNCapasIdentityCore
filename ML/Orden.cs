using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Orden
    {
        public int IdOrden { get; set; }
        public ML.UserIdentity Usuario { get; set; }
        public ML.Producto Producto { get; set; }
        public int Cantidad { get; set; }

    }
}
