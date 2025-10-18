using System.Collections.Generic;
using BE;
using MPP;

namespace BLL
{
    public class BLLPublicidad
    {
        private readonly MPPPublicidad oMPPPublicidad;
        public BLLPublicidad() { oMPPPublicidad = new MPPPublicidad(); }

        public int Guardar(BEPublicidad pub) => oMPPPublicidad.Guardar(pub);
        public bool Eliminar(BEPublicidad pub) => oMPPPublicidad.Eliminar(pub);
        public BEPublicidad ObtenerPorId(BEPublicidad pub) => oMPPPublicidad.ObtenerPorId(pub);
        public List<BEPublicidad> ListarActivas() => oMPPPublicidad.ListarActivas();
        public List<BEPublicidad> ListarAdmin() => oMPPPublicidad.ListarAdmin();

    }
}