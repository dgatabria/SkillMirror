using BE;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLBitacora
    {
        private MPPBitacora mpp;

        public BLLBitacora()
        {
            mpp = new MPPBitacora();
        }

        public List<EntradaBitacora> Listar(DateTime? fechaDesde, DateTime? fechaHasta, string accion, string detalle, int pageIndex, int pageSize, out int totalRecords)
        {
            return mpp.Listar(fechaDesde, fechaHasta, accion, detalle, pageIndex, pageSize, out totalRecords);
        }
    }
}
