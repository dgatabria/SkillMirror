using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class BERespuesta
    {
        public int Codigo { get; set; }
        public BEEncuestaInvitacion Invitacion { get; set; }
        public BEPregunta Pregunta { get; set; }
        public string RespuestaTexto { get; set; }
        public int? Puntuacion { get; set; }
        public BEOpcionRespuesta OpcionSeleccionada { get; set; }

        public BERespuesta()
        {
            Invitacion = new BEEncuestaInvitacion();
            Pregunta = new BEPregunta();
            OpcionSeleccionada = new BEOpcionRespuesta();
        }
    }

    [Serializable]
    public class BEEncuestaInvitacion
    {
        public int Codigo { get; set; }
        public BEEncuesta Encuesta { get; set; }
        public BEUsuario Usuario { get; set; }
        public Guid Token { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Respondida { get; set; }

        public BEEncuestaInvitacion()
        {
            Encuesta = new BEEncuesta();
            Usuario = new BEUsuario();
        }
    }

    [Serializable]
    public class BEOpcionRespuesta
    {
        public int Codigo { get; set; }
        public string TextoOpcion { get; set; }
        public int Orden { get; set; }
    }
    [Serializable]
    public class BEPregunta
    {
        public int Codigo { get; set; }
        public string TextoPregunta { get; set; }
        public string TipoPregunta { get; set; } // 'TEXTO_LIBRE', 'PUNTAJE_1_5', 'OPCION_MULTIPLE'
        public List<BEOpcionRespuesta> Opciones { get; set; }

        public BEEncuesta EncuestaPadre {  get; set; }
        public BEPregunta()
        {
            Opciones = new List<BEOpcionRespuesta>();
        }
    }

    [Serializable]
    public class BEEncuesta
    {
        public int Codigo { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool Activa { get; set; }
        public List<BEPregunta> Preguntas { get; set; }

        public BEEncuesta()
        {
            Preguntas = new List<BEPregunta>();
        }
    }
}
