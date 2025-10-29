using BE;
using Mapper;
using System;
using System.Collections.Generic;
using MPP;

namespace BLL
{
    /// <summary>
    /// La capa BLL (Business Logic Layer) para la gestión de Planes.
    /// Sirve como intermediario entre la capa de presentación (UI) y la capa de mapeo de datos (Mapper).
    /// </summary>
    public class BLLPlanes
    {
        private MPPPlanes oMapper;

        public BLLPlanes()
        {
            oMapper = new MPPPlanes();
        }

        /// <summary>
        /// Llama al Mapper para obtener la lista de planes públicos.
        /// </summary>
        public List<BEPlan> Listar()
        {
            return oMapper.Listar();
        }

        /// <summary>
        /// Llama al Mapper para obtener la lista de planes para el panel de administración.
        /// </summary>
        public List<BEPlan> ListarAdmin()
        {
            return oMapper.ListarAdmin();
        }

        /// <summary>
        /// Llama al Mapper para obtener un plan específico por su ID.
        /// </summary>
        public BEPlan ListarPorID(int id)
        {
            return oMapper.ListarPorID(id);
        }

        /// <summary>
        /// Llama al Mapper para guardar (crear o actualizar) un plan.
        /// </summary>
        public int Guardar(BEPlan plan)
        {
            return oMapper.Guardar(plan);
        }

        /// <summary>
        /// Llama al Mapper para realizar la baja lógica de un plan por su ID.
        /// </summary>
        public void BajaLogica(int id)
        {
            oMapper.BajaLogica(id);
        }
    }
}

