using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Producto
    {
		public int Id { get; set; }
		public string? Nombre { get; set; }
		public string? Descripcion { get; set; }
		public int? Stock { get; set; }
        public int? EnCurso { get; set; }
        public decimal? Precio { get; set; }
        public byte[]? Imagen { get; set; }
        public ML.Departamento? Departamento { get; set; }
        public List<Object>? Productos { get; set; }
    }
}
