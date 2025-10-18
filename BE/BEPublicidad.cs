using System;

namespace BE
{
    [Serializable]
    public class BEPublicidad
    {
        public int Codigo { get; set; }
        public string Titulo { get; set; }
        public string URLDestino { get; set; }
        public string RutaImagen { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime FechaExpiracion { get; set; } = DateTime.Now.AddMonths(1);
        public bool Activo { get; set; } = true;
        public int ContadorClicks { get; set; }
    }
}