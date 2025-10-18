using System.Collections.Generic;
using BE;
using MPP;

namespace BLL
{
    public class BLLResena
    {
        private readonly MPPResena oMPPResena;

        public BLLResena()
        {
            oMPPResena = new MPPResena();
        }

        public List<BEResena> ListarTodas()
        {
            return oMPPResena.ListarTodas();
        }

        public bool Aprobar(int resenaId)
        {
            return oMPPResena.Aprobar(resenaId);
        }

        public bool Eliminar(int resenaId)
        {
            return oMPPResena.Eliminar(resenaId);
        }
        public BERankingEstadisticas ObtenerEstadisticas()
        {
            return oMPPResena.ObtenerEstadisticas();
        }
        public bool Guardar(BEResena resena)
        {

            if (resena.Puntuacion < 1 || resena.Puntuacion > 5)
                throw new System.Exception("La puntuación debe estar entre 1 y 5.");

            return oMPPResena.Guardar(resena);
        }

        public List<BEResena> ListarAprobadas()
        {
            return oMPPResena.ListarAprobadas();
        }
    }
}