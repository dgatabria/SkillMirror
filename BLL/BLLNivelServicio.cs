using System.Collections.Generic;
using BE;
using MPP;

namespace BLL
{
    public class BLLNivelServicio
    {
        private readonly MPPNivelServicio oMPPNivelServicio;
        public BLLNivelServicio() { oMPPNivelServicio = new MPPNivelServicio(); }

        public List<BENivelServicio> Listar()
        {
            return oMPPNivelServicio.Listar();
        }
        public BENivelServicio ObtenerPorId(int id)
        {
            return oMPPNivelServicio.ObtenerPorId(id);
        }
    }
}