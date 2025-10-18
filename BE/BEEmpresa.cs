using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class BEEmpresa
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Domicilio { get; set; }
        public string MedioDePago { get; set; }
        public string CUIT { get; set; }

        // --- PROPIEDAD NUEVA ---
        // Usamos un objeto completo para tener toda la info del plan (nombre, costo, etc.)
        public BENivelServicio PlanContratado { get; set; }
    }

    [Serializable]
    public class BENivelServicio
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Gratis { get; set; }
        public double CostoMensual { get; set; }
    }
}
