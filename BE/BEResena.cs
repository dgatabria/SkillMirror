using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace BE
{
    public class BEResena
    {
        public int Codigo { get; set; }
        public BEUsuario Autor { get; set; }
        public int Puntuacion { get; set; }
        public string Asunto { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
        public bool Aprobado { get; set; }

        public BEResena()
        {
            Autor = new BEUsuario();
        }
    }
}