using BE;
using MPP;
using System.Collections.Generic;

namespace BLL
{
    public class BLLBusqueda
    {
        private readonly MPPBusqueda _mppBusqueda;
        public BLLBusqueda()
        {
            _mppBusqueda = new MPPBusqueda();
        }

        public List<BEResultadoBusqueda> Buscar(string termino, bool esPublica, int idIdioma)
        {
            // Aquí se podrían agregar reglas de negocio, como loguear los términos más buscados.
            if (string.IsNullOrWhiteSpace(termino))
            {
                return new List<BEResultadoBusqueda>();
            }
            return _mppBusqueda.Buscar(termino, esPublica, idIdioma);
        }
    }
}
