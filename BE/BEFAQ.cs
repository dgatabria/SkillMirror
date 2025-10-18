using System;
using System.Collections.Generic;

namespace BE
{
    [Serializable]
    public class BEFAQ
    {
        public int Codigo { get; set; }

        // Texto para la vista de listado (en el idioma actual)
        public string Pregunta { get; set; }
        public string Respuesta { get; set; }

        public int Orden { get; set; }
        public bool Activo { get; set; }

        // Tags para vincular con la tabla de traducciones
        public string PreguntaTag { get; set; }
        public string RespuestaTag { get; set; }

        // Diccionarios para manejar TODAS las traducciones en la pantalla de edición
        public Dictionary<int, string> TraduccionesPregunta { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, string> TraduccionesRespuesta { get; set; } = new Dictionary<int, string>();
    }
}