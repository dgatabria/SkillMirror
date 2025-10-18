using System;

namespace BE
{
    [Serializable]
    public class BEEstadisticasGanancias
    {
        public decimal GananciasHoy { get; set; }
        public decimal GananciasSemana { get; set; }
        public decimal GananciasMes { get; set; }
        public decimal GananciasAnio { get; set; }
        public BEEstadisticasGanancias()
        {
            GananciasHoy = 0;
            GananciasSemana = 0;
            GananciasMes = 0;
            GananciasAnio = 0;
        }
    }
}
