using System;
using System.Collections.Generic;

namespace BE
{
    [Serializable]
    public class BEResultadoPregunta
    {
        public int PreguntaID { get; set; }
        public string TextoPregunta { get; set; }
        public string TipoPregunta { get; set; }

        // Para preguntas de opción múltiple o puntaje
        public Dictionary<string, int> ConteoOpciones { get; set; }

        // Para preguntas de texto libre
        public List<string> RespuestasAbiertas { get; set; }

        public BEResultadoPregunta()
        {
            ConteoOpciones = new Dictionary<string, int>();
            RespuestasAbiertas = new List<string>();
        }
    }

    [Serializable]
    public class BEResultadoEncuesta
    {
        public string TituloEncuesta { get; set; }
        public int TotalRespuestas { get; set; }
        public List<BEResultadoPregunta> ResultadosPorPregunta { get; set; }

        public BEResultadoEncuesta()
        {
            ResultadosPorPregunta = new List<BEResultadoPregunta>();
        }
    }
}