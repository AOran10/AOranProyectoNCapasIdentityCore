using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Estatus
    {
        public int? IdEstatus { get; set; }
        public string? NombreEstatus { get; set; }
        public string? Descripcion { get; set; }
        public List<object>? EstatusList { get; set; }
    }
}
