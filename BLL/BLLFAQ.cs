using System.Collections.Generic;
using BE;
using MPP;

namespace BLL
{
    public class BLLFAQ
    {
        private readonly MPPFAQ oMPPFAQ;
        private readonly Traductor _traductor;

        public BLLFAQ()
        {
            oMPPFAQ = new MPPFAQ();
            _traductor = Traductor.ObtenerInstancia(); // Obtenemos la instancia del traductor
        }

        public int Guardar(BEFAQ faq)
        {
            return oMPPFAQ.Guardar(faq);
        }

        public bool Baja(BEFAQ faq)
        {
            return oMPPFAQ.Baja(faq);
        }

        public List<BEFAQ> ListarAdmin()
        {
            // Ahora pasamos automáticamente el idioma actual a la capa de datos
            return oMPPFAQ.ListarAdmin(_traductor.IdiomaSeleccionado.Codigo);
        }

        public List<BEFAQ> ListarPublicas()
        {
            // También aquí pasamos el idioma actual
            return oMPPFAQ.ListarPublicas(_traductor.IdiomaSeleccionado.Codigo);
        }

        public BEFAQ ListarObjetoConTraducciones(BEFAQ faq)
        {
            return oMPPFAQ.ListarObjetoConTraducciones(faq);
        }
    }
}