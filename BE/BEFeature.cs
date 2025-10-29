using System;

namespace BE
{
    /// <summary>
    /// Representa una característica comparable entre planes (ej: "Procesos de Selección").
    /// </summary>
    public class BEFeature
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public int Orden { get; set; }
    }
}