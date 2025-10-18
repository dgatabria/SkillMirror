using BE;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLEmpresa
    {
        private MPPEmpresa mpp;

        public BLLEmpresa()
        {
            mpp = new MPPEmpresa();
        }

        public int Guardar(BEEmpresa empresa)
        {
            return mpp.Guardar(empresa);
        }
        public bool ActualizarPlan(int idEmpresa, int idPlan)
        {
            return mpp.ActualizarPlan(idEmpresa, idPlan);
        }
        public bool Baja(BEEmpresa empresa)
        {
            return mpp.Baja(empresa);
        }
        public BEEmpresa  ListarObjeto(BEEmpresa empresa)
        {
            return mpp.ListarObjeto(empresa);
        }
        public List<BEEmpresa> ListarTodos()
        {
            return mpp.ListarTodos();
        }
    }
}