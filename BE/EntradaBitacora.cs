using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class EntradaBitacora
    {
        public int ID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string NombreUsuario { get; set; } // Traeremos el nombre, no el ID
        public string IP { get; set; }
        public string Detalle { get; set; }
        public string Accion { get; set; }
        public int Criticidad { get; set; }
    }
}
