using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class BEIdioma
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public Hashtable Palabras { get; set; }

        public BEIdioma()
        {
            Palabras = new Hashtable();
        }
        public BEIdioma(int Codigo)
        {
            this.Codigo = Codigo;
            Palabras = new Hashtable();
        }
        public override string ToString()
        {
            return Nombre;
        }
    }
}
