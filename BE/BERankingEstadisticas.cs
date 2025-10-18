using System;
using System.Collections.Generic;

namespace BE
{
    [Serializable]
    public class BERankingEstadisticas
    {
        public decimal PromedioPuntuacion { get; set; }
        public int TotalResenas { get; set; }
        // Usamos un diccionario para guardar la distribución: <Estrellas, Cantidad>
        public Dictionary<int, int> DistribucionEstrellas { get; set; }
        public List<BEResena> ResenasDestacadas { get; set; }

        public BERankingEstadisticas()
        {
            DistribucionEstrellas = new Dictionary<int, int>();
            ResenasDestacadas = new List<BEResena>();
        }
    }
}