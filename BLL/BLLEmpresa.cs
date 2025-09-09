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

        public bool Guardar(BEEmpresa empresa)
        {
            return mpp.Guardar(empresa);
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