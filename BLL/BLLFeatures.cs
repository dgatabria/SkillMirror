using BE;
using Mapper;
using System.Collections.Generic;

namespace BLL
{
    public class BLLFeatures
    {
        private MPPFeatures oMapper;

        public BLLFeatures()
        {
            oMapper = new MPPFeatures();
        }

        /// <summary>
        /// Devuelve una lista de todas las características disponibles.
        /// </summary>
        public List<BEFeature> Listar()
        {
            return oMapper.Listar();
        }
    }
}