using System;
using System.Collections.Generic;

namespace BE
{


    /// <summary>
    /// Representa la descripción de una característica específica para un plan.
    /// Ej: Para el Plan "Basic" y la Feature "procesos", la descripción sería "Hasta 5 procesos activos".
    /// </summary>
    [Serializable]
    public class BEPlanFeature
    {
        public int ID { get; set; } // ID de la relación PlanFeature
        public int ID_Feature { get; set; }
        public string FeatureNombre { get; set; } // Nombre legible de la Feature
        public string Descripcion { get; set; }

        // Propiedad adicional para la lógica de la UI
        public string Clave { get; set; }
    }

    /// <summary>
    /// Representa un plan de suscripción con sus características.
    /// </summary>
    [Serializable]
    public class BEPlan
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Subtitulo { get; set; }
        public decimal PrecioMensual { get; set; }
        public bool Activo { get; set; }
        public int Orden { get; set; }
        public bool EsDestacado { get; set; }

        /// <summary>
        /// Lista de características detalladas para este plan.
        /// </summary>
        public List<BEPlanFeature> Features { get; set; }

        public BEPlan()
        {
            // Inicializamos la lista para evitar errores de referencia nula.
            Features = new List<BEPlanFeature>();
        }
    }
}
