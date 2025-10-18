using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class BENovedad
    {
        public int Codigo { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public BEUsuario Autor { get; set; }
        public bool Activo { get; set; }

        public BENovedad()
        {
            Autor = new BEUsuario();
        }

    }
}
