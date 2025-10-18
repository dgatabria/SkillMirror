using System;
using System.Collections.Generic;

namespace BE
{
    [Serializable]
    public class BEMensaje
    {
        public int Codigo { get; set; }
        public int ConversacionID { get; set; } // Para vincularlo al hilo
        public BEUsuario Remitente { get; set; }
        public string CuerpoMensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Leido { get; set; }

        public BEMensaje()
        {
            Remitente = new BEUsuario();
            FechaEnvio = DateTime.Now;
        }
    }

    [Serializable]
    public class BEConversacion
    {
        public int Codigo { get; set; }
        public BEUsuario Usuario { get; set; } // Quien inicia la conversación
        public string Asunto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } // "Abierto", "Respondido por Agente", "Cerrado"
        public List<BEMensaje> Mensajes { get; set; }

        // Propiedades auxiliares para mostrar en la grilla del administrador
        public DateTime FechaUltimoMensaje { get; set; }
        public int MensajesNuevos { get; set; }


        public BEConversacion()
        {
            Usuario = new BEUsuario();
            Mensajes = new List<BEMensaje>();
            FechaCreacion = DateTime.Now;
            Estado = "Abierto";
        }
    }
}