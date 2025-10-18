using System;

namespace BE
{
    [Serializable]
    public class BEResultadoBusqueda
    {
        public string Tipo { get; set; }
        public string Titulo { get; set; }
        public string URL { get; set; }
        public string Descripcion { get; set; }
    }
}
